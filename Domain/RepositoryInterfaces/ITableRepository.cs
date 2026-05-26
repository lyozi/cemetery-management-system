using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.RepositoryInterfaces
{
  public interface ITableRepository
  {
    Task<List<Table>> GetAllAsync();
    Task<Table?> GetByIdAsync(short id);
    Task<Table> UpsertAsync(short id, string? name, List<TablePoint> latLngs);
    Task<bool> DeleteAsync(short id);
    Task<List<Grave>> GetGravesWithPolygonsAsync();
    void MarkGraveModified(Grave grave);
    Task SaveAsync();
  }
}
