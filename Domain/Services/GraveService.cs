using Domain.Models;
using Domain.RepositoryInterfaces;
using Domain.ServiceInterfaces;
using System.Collections.Generic;

namespace Domain.Services
{
    public class GravesService : IGravesService
    {
        private readonly IGraveRepository _graveRepository;

        public GravesService(IGraveRepository graveRepository)
        {
            _graveRepository = graveRepository;
        }

        public IEnumerable<Grave> GetGraves()
        {
            return _graveRepository.GetGraves();
        }

        public Grave GetGraveByID(long id)
        {
            return _graveRepository.GetGraveByID(id);
        }

        public void InsertGrave(Grave grave)
        {
            _graveRepository.InsertGrave(grave);
            _graveRepository.Save();
        }

        public void UpdateGrave(Grave grave)
        {
            _graveRepository.UpdateGrave(grave);
            _graveRepository.Save();
        }

        public void DeleteGrave(long id)
        {
            _graveRepository.DeleteGrave(id);
            _graveRepository.Save();
        }

        public bool GraveExists(long id)
        {
            return _graveRepository.GraveExists(id);
        }

        public Grave GetOrCreateGrave(char table, short row, short parcel)
        {
            var grave = _graveRepository.GetGraveByTableRowParcel(table, row, parcel);
            if (grave == null)
            {
                grave = new Grave
                {
                    Table = table,
                    Row = row,
                    Parcel = parcel
                };
                _graveRepository.InsertGrave(grave);
                _graveRepository.Save();
            }
            return grave;
        }

        public Grave GetGraveFromPolygonId(long id)
        {
            return _graveRepository.GetGraveFromPolygonId(id);
        }
    }
}