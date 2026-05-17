using System.Collections.Generic;

namespace Domain.DTOs
{
  public class BulkRowAssignDTO
  {
    public List<long> PolygonIds { get; set; } = new();
    public short Row { get; set; }
  }
}
