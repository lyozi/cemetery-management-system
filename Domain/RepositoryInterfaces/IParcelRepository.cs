using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.RepositoryInterfaces
{
  public interface IParcelRepository
  {
    Task<List<Parcel>> GetAllAsync();
    Task<Parcel?> GetByIdAsync(short id);
    Task<Parcel> UpsertAsync(short id, string? name, List<ParcelPoint> latLngs);
    Task<bool> DeleteAsync(short id);
    Task<List<Grave>> GetGravesWithPolygonsAsync();
    void MarkGraveModified(Grave grave);
    Task SaveAsync();
  }
}
