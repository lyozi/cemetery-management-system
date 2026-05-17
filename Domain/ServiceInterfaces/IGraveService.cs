using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.ServiceInterfaces
{
    public interface IGravesService
    {
        IEnumerable<Grave> GetGraves();
        Grave GetGraveByID(long id);
        void InsertGrave(Grave grave);
        void UpdateGrave(Grave grave);
        Task DeleteGraveAsync(long id, CancellationToken ct = default);
        Task DeleteAllGravesAsync(CancellationToken ct = default);
        bool GraveExists(long id);
        Grave GetOrCreateGrave(char table, short row, short parcel);
        Grave GetGraveFromPolygonId(long id);
        int BulkAssignRow(IEnumerable<long> polygonIds, short row);

        Task<string> SetGraveImageAsync(
            long graveId,
            Stream content,
            string fileName,
            string contentType,
            CancellationToken ct = default);

        Task DeleteGraveImageAsync(long graveId, CancellationToken ct = default);
    }
}
