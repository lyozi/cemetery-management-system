using Domain.Models;
using System;
using System.Collections.Generic;

namespace Infrastructure.GraveRepo
{
    public interface IGraveRepository : IDisposable
    {
        IEnumerable<Grave> GetGraves();
        Grave GetGraveByID(long id);
        void InsertGrave(Grave grave);
        void DeleteGrave(long graveID);
        void UpdateGrave(Grave grave);
        bool GraveExists(long id);
        void Save();
    }
}
