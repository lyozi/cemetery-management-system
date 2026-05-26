using Domain.Models;

namespace Domain.DTOs
{
  public class TableAssignmentResultDTO
  {
    public required Table Table { get; set; }
    public int AssignedCount { get; set; }
    public int UnassignedCount { get; set; }
  }
}
