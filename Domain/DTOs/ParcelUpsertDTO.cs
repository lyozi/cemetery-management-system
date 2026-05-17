using System.Collections.Generic;

namespace Domain.DTOs
{
  public class ParcelUpsertDTO
  {
    public short Id { get; set; }
    public string? Name { get; set; }
    public List<ParcelPointDTO> LatLngs { get; set; } = new();
  }

  public class ParcelPointDTO
  {
    public double Lat { get; set; }
    public double Lng { get; set; }
  }
}
