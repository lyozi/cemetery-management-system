using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.DTOs;
using Domain.Models;

namespace Domain.ServiceInterfaces
{
  public interface IParcelsService
  {
    Task<List<Parcel>> GetAllAsync();
    Task<Parcel?> GetByIdAsync(short id);
    Task<ParcelAssignmentResultDTO> UpsertAndAssignAsync(short id, string? name, List<ParcelPointDTO> latLngs);
    Task<bool> DeleteAsync(short id);
  }
}
