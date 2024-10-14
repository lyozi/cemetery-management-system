using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models;
using Domain.RepositoryInterfaces;
using Domain.ServiceInterfaces;


namespace Domain.Services
{
    public class DeceasedService : IDeceasedService
    {
        private readonly IDeceasedRepository _deceasedRepository;

        public DeceasedService(IDeceasedRepository deceasedRepository)
        {
            _deceasedRepository = deceasedRepository;
        }

        public IEnumerable<Deceased> GetDeceaseds()
        {
            return _deceasedRepository.GetDeceaseds();
        }

        public IEnumerable<Deceased> SearchDeceaseds(string name, int? birthYearAfter, int? deceaseYearBefore, string orderBy)
        {
            var query = _deceasedRepository.GetDeceaseds();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(d => d.Name.ToUpper().Contains(name.ToUpper()));
            }

            if (birthYearAfter.HasValue && birthYearAfter > 0)
            {
                query = query.Where(d => d.DateOfBirth.Year >= birthYearAfter);
            }

            if (deceaseYearBefore.HasValue && deceaseYearBefore > 0)
            {
                query = query.Where(d => d.DateOfDeath.Year <= deceaseYearBefore);
            }

            if (!string.IsNullOrEmpty(orderBy))
            {
                switch (orderBy.ToLower())
                {
                    case "name_asc":
                        query = query.OrderBy(d => d.Name);
                        break;
                    case "name_desc":
                        query = query.OrderByDescending(d => d.Name);
                        break;
                    case "dateofdeath_asc":
                        query = query.OrderBy(d => d.DateOfDeath);
                        break;
                    case "dateofdeath_desc":
                        query = query.OrderByDescending(d => d.DateOfDeath);
                        break;
                    case "dateofbirth_asc":
                        query = query.OrderBy(d => d.DateOfBirth);
                        break;
                    case "dateofbirth_desc":
                        query = query.OrderByDescending(d => d.DateOfBirth);
                        break;
                    default:
                        break;
                }
            }

            return query.ToList();
        }

        public Deceased GetDeceasedByID(long id)
        {
            return _deceasedRepository.GetDeceasedByID(id);
        }

        public async Task<Deceased> GetDeceasedWithMessagesByID(long id)
        {
            return await _deceasedRepository.GetDeceasedWithMessagesByID(id);
        }

        public void AddMessageToDeceased(long id, Message message)
        {
            var deceased = _deceasedRepository.GetDeceasedByID(id);
            if (deceased != null)
            {
                if (deceased.MessageList == null)
                {
                    deceased.MessageList = new List<Message>();
                }

                deceased.MessageList.Add(message);

                _deceasedRepository.Save();
            }
        }

        public bool InsertDeceased(Deceased deceased)
        {
            bool isSuccessful = _deceasedRepository.InsertDeceased(deceased);
            _deceasedRepository.Save();
            return isSuccessful;
        }

        public void UpdateDeceased(Deceased deceased)
        {
            _deceasedRepository.UpdateDeceased(deceased);
            _deceasedRepository.Save();
        }

        public void DeleteDeceased(long id)
        {
            _deceasedRepository.DeleteDeceased(id);
            _deceasedRepository.Save();
        }

        public bool DeceasedExists(long id)
        {
            return _deceasedRepository.DeceasedExists(id);
        }
    }
}
