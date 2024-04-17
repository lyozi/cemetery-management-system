using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DeceasedRepo
{
    public interface IDeceasedRepository : IDisposable
    {
        IEnumerable<Deceased> GetDeceaseds();
        Deceased GetDeceasedByID(long deceasedId);
        public Task<Deceased> GetDeceasedWithMessagesByID(long id);
        void InsertDeceased(Deceased deceased);
        void DeleteDeceased(long deceasedID);
        void UpdateDeceased(Deceased deceased);
        public bool DeceasedExists(long id);
        void Save();
    }
}
