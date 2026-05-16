using Domain.Models;

namespace Domain.RepositoryInterfaces
{
    public interface IGraveRepository : IDisposable
    {
        IEnumerable<Grave> GetGraves();
        IEnumerable<Grave> GetGravesWithDeceasedList();
        Grave GetGraveByID(long id);
        Grave? GetGraveWithDeceasedListByID(long id);
        void InsertGrave(Grave grave);
        void DeleteGrave(long graveID);
        void UpdateGrave(Grave grave);
        bool GraveExists(long id);
        Grave GetGraveFromPolygonId(long id);
        Grave GetGraveByTableRowParcel(char table, short row, short parcel);
        void Save();
    }
}
