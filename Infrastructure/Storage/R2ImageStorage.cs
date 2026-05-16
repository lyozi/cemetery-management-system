using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Domain.ServiceInterfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Storage
{
    public class R2ImageStorage : IImageStorage, IDisposable
    {
        private static readonly HashSet<string> AllowedExt =
            new(StringComparer.OrdinalIgnoreCase) { ".jpg", ".jpeg", ".png", ".webp" };
        private const long MaxBytes = 10L * 1024 * 1024;

        private readonly R2Options _opts;
        private readonly IAmazonS3 _s3;
        private readonly ILogger<R2ImageStorage> _log;

        public R2ImageStorage(IOptions<R2Options> opts, ILogger<R2ImageStorage> log)
        {
            _opts = opts.Value;
            _log = log;
            _s3 = new AmazonS3Client(
                _opts.AccessKeyId,
                _opts.SecretAccessKey,
                new AmazonS3Config
                {
                    ServiceURL = $"https://{_opts.AccountId}.r2.cloudflarestorage.com",
                    ForcePathStyle = true
                });
        }

        public async Task<string> UploadAsync(
            Stream content,
            string fileName,
            string contentType,
            string keyPrefix,
            CancellationToken ct = default)
        {
            if (content is null)
                throw new ArgumentException("Content stream is null.", nameof(content));
            if (content.CanSeek && content.Length == 0)
                throw new ArgumentException("Empty content.", nameof(content));
            if (content.CanSeek && content.Length > MaxBytes)
                throw new ArgumentException("File exceeds 10 MB.", nameof(content));

            var ext = Path.GetExtension(fileName);
            if (string.IsNullOrEmpty(ext) || !AllowedExt.Contains(ext))
                throw new ArgumentException("Unsupported image type.", nameof(fileName));

            var key = $"{keyPrefix.Trim('/')}/{Guid.NewGuid():N}{ext.ToLowerInvariant()}";

            var put = new PutObjectRequest
            {
                BucketName = _opts.BucketName,
                Key = key,
                InputStream = content,
                ContentType = string.IsNullOrWhiteSpace(contentType) ? "application/octet-stream" : contentType,
                DisablePayloadSigning = true
            };

            await _s3.PutObjectAsync(put, ct);

            return $"{_opts.PublicBaseUrl.TrimEnd('/')}/{key}";
        }

        public async Task DeleteAsync(string urlOrKey, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(urlOrKey)) return;

            var prefix = _opts.PublicBaseUrl.TrimEnd('/') + "/";
            var key = urlOrKey.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)
                ? urlOrKey.Substring(prefix.Length)
                : urlOrKey;

            try
            {
                await _s3.DeleteObjectAsync(new DeleteObjectRequest
                {
                    BucketName = _opts.BucketName,
                    Key = key
                }, ct);
            }
            catch (AmazonS3Exception ex) when (ex.ErrorCode == "NoSuchKey")
            {
                // Already gone — fine.
            }
            catch (Exception ex)
            {
                _log.LogWarning(ex, "R2 delete failed for key {Key}. Continuing.", key);
            }
        }

        public void Dispose()
        {
            _s3?.Dispose();
        }
    }
}
