using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.DTOs;
using Domain.Models;

namespace Domain.ServiceInterfaces
{
  public interface ITablesService
  {
    Task<List<Table>> GetAllAsync();
    Task<Table?> GetByIdAsync(short id);
    Task<TableAssignmentResultDTO> UpsertAndAssignAsync(short id, string? name, List<TablePointDTO> latLngs);
    Task<bool> DeleteAsync(short id);
  }
}
