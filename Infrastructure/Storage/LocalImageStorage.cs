using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Domain.ServiceInterfaces;
using Microsoft.Extensions.Options;

namespace Infrastructure.Storage
{
    public class LocalImageStorageOptions
    {
        public string RootPath { get; set; } = "wwwroot/uploads";
        public string PublicPath { get; set; } = "/uploads";
        public string PublicBaseUrl { get; set; } = "";
    }

    public class LocalImageStorage : IImageStorage
    {
        private static readonly HashSet<string> AllowedExt =
            new(StringComparer.OrdinalIgnoreCase) { ".jpg", ".jpeg", ".png", ".webp" };
        private const long MaxBytes = 10L * 1024 * 1024;

        private readonly LocalImageStorageOptions _opts;

        public LocalImageStorage(IOptions<LocalImageStorageOptions> opts)
        {
            _opts = opts.Value;
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

            var prefix = keyPrefix.Trim('/');
            var file = $"{Guid.NewGuid():N}{ext.ToLowerInvariant()}";
            var relKey = $"{prefix}/{file}";

            var dir = Path.Combine(_opts.RootPath, prefix);
            Directory.CreateDirectory(dir);

            var fullPath = Path.Combine(_opts.RootPath, prefix, file);
            await using var fs = new FileStream(fullPath, FileMode.CreateNew, FileAccess.Write, FileShare.None);
            await content.CopyToAsync(fs, ct);

            return $"{_opts.PublicBaseUrl.TrimEnd('/')}{_opts.PublicPath.TrimEnd('/')}/{relKey}";
        }

        public Task DeleteAsync(string urlOrKey, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(urlOrKey)) return Task.CompletedTask;

            var marker = _opts.PublicPath.TrimEnd('/') + "/";
            var idx = urlOrKey.IndexOf(marker, StringComparison.OrdinalIgnoreCase);
            var key = idx >= 0 ? urlOrKey.Substring(idx + marker.Length) : urlOrKey;

            var path = Path.Combine(_opts.RootPath, key.Replace('/', Path.DirectorySeparatorChar));
            if (File.Exists(path)) File.Delete(path);

            return Task.CompletedTask;
        }
    }
}
