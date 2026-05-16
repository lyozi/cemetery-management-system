using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.ServiceInterfaces
{
    public interface IImageStorage
    {
        Task<string> UploadAsync(
            Stream content,
            string fileName,
            string contentType,
            string keyPrefix,
            CancellationToken ct = default);

        Task DeleteAsync(string urlOrKey, CancellationToken ct = default);
    }
}
