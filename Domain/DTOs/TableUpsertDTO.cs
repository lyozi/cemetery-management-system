using System.Collections.Generic;

namespace Domain.DTOs
{
  public class TableUpsertDTO
  {
    public short Id { get; set; }
    public string? Name { get; set; }
    public List<TablePointDTO> LatLngs { get; set; } = new();
  }

  public class TablePointDTO
  {
    public double Lat { get; set; }
    public double Lng { get; set; }
  }
}
