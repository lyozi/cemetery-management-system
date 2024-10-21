using Domain.Models;

namespace Domain.ServiceInterfaces
{
    public interface IGravesService
    {
        IEnumerable<Grave> GetGraves();
        Grave GetGraveByID(long id);
        void InsertGrave(Grave grave);
        void UpdateGrave(Grave grave);
        void DeleteGrave(long id);
        bool GraveExists(long id);
        Grave GetOrCreateGrave(char table, short row, short parcel);
        Grave GetGraveFromPolygonId(long id);
    }
}
