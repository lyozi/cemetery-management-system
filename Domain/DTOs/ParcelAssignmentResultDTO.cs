using Domain.Models;

namespace Domain.DTOs
{
  public class ParcelAssignmentResultDTO
  {
    public required Parcel Parcel { get; set; }
    public int AssignedCount { get; set; }
    public int UnassignedCount { get; set; }
  }
}
