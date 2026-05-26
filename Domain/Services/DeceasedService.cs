using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.DTOs;
using Domain.Models;
using Domain.RepositoryInterfaces;
using Domain.ServiceInterfaces;
using Microsoft.Extensions.Logging;

namespace Domain.Services
{
    public class DeceasedService : IDeceasedService
    {
        private readonly IDeceasedRepository _deceasedRepository;
        private readonly IGravesService _gravesService;
        private readonly IImageStorage _imageStorage;
        private readonly ILogger<DeceasedService> _log;

        public DeceasedService(
            IDeceasedRepository deceasedRepository,
            IGravesService gravesService,
            IImageStorage imageStorage,
            ILogger<DeceasedService> log)
        {
            _deceasedRepository = deceasedRepository;
            _gravesService = gravesService;
            _imageStorage = imageStorage;
            _log = log;
        }

        public IEnumerable<Deceased> GetDeceaseds()
        {
            return _deceasedRepository.GetDeceaseds();
        }

        public (IEnumerable<Deceased> Items, int TotalCount) SearchDeceaseds(
            string name,
            int? birthYearAfter,
            int? deceaseYearBefore,
            string orderBy,
            int pageNumber,
            int pageSize)
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

            var totalCount = query.Count();
            var skip = (pageNumber - 1) * pageSize;
            var items = query.Skip(skip).Take(pageSize).ToList();

            return (items, totalCount);
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

        public async Task DeleteDeceasedAsync(long id, CancellationToken ct = default)
        {
            var deceased = _deceasedRepository.GetDeceasedByID(id);
            if (deceased is null) return;

            var url = deceased.ImageUrl;
            _deceasedRepository.DeleteDeceased(id);
            _deceasedRepository.Save();

            if (!string.IsNullOrEmpty(url))
            {
                try
                {
                    await _imageStorage.DeleteAsync(url, ct);
                }
                catch (Exception ex)
                {
                    _log.LogWarning(ex, "Image cleanup failed for {Url}", url);
                }
            }
        }

        public bool DeceasedExists(long id)
        {
            return _deceasedRepository.DeceasedExists(id);
        }

        public Deceased CreateDeceased(DeceasedDataDTO deceasedData)
        {
            Grave deceasedsGrave = _gravesService.GetOrCreateGrave(deceasedData.GraveTable, deceasedData.GraveRow);
            var deceased = new Deceased
            {
                Name = deceasedData.Name,
                DateOfDeath = deceasedData.DateOfDeath,
                DateOfBirth = deceasedData.DateOfBirth,
                ImageUrl = deceasedsGrave.ImageUrl,
                Grave = deceasedsGrave,
                GraveId = deceasedsGrave.Id
            };
            InsertDeceased(deceased);
            return deceased;
        }

        public IEnumerable<Deceased> CreateDeceaseds(IEnumerable<DeceasedDataDTO> deceasedDataList)
        {
            var deceasedList = new List<Deceased>();

            foreach (var deceasedData in deceasedDataList)
            {
                var deceased = CreateDeceased(deceasedData);
                deceasedList.Add(deceased);
            }

            return deceasedList;
        }
    }
}
