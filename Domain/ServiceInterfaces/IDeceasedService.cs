using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.DTOs;
using Domain.Models;

namespace Domain.ServiceInterfaces
{
    public interface IDeceasedService
    {
        IEnumerable<Deceased> GetDeceaseds();
        (IEnumerable<Deceased> Items, int TotalCount) SearchDeceaseds(string name,
        int? birthYearAfter,
        int? deceaseYearBefore,
        string orderBy,
        int pageNumber,
        int pageSize);
        Deceased GetDeceasedByID(long id);
        Task<Deceased> GetDeceasedWithMessagesByID(long id);
        void AddMessageToDeceased(long id, Message message);
        bool InsertDeceased(Deceased deceased);
        void UpdateDeceased(Deceased deceased);
        Task DeleteDeceasedAsync(long id, CancellationToken ct = default);
        bool DeceasedExists(long id);
        Deceased CreateDeceased(DeceasedDataDTO deceasedData);
        IEnumerable<Deceased> CreateDeceaseds(IEnumerable<DeceasedDataDTO> deceasedDataList);
    }
}
