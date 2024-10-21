using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.DTOs;
using Domain.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Domain.ServiceInterfaces
{
    public interface IDeceasedService
    {
        IEnumerable<Deceased> GetDeceaseds();
        IEnumerable<Deceased> SearchDeceaseds(string name, int? birthYearAfter, int? deceaseYearBefore, string orderBy);
        Deceased GetDeceasedByID(long id);
        Task<Deceased> GetDeceasedWithMessagesByID(long id);
        void AddMessageToDeceased(long id, Message message);
        bool InsertDeceased(Deceased deceased);
        void UpdateDeceased(Deceased deceased);
        void DeleteDeceased(long id);
        bool DeceasedExists(long id);
        Deceased CreateDeceased(DeceasedDataDTO deceasedData);
        IEnumerable<Deceased> CreateDeceaseds(IEnumerable<DeceasedDataDTO> deceasedDataList);

    }
}
