using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
  public class Parcel
  {
    [Key]
    public short Id { get; set; }

    public string? Name { get; set; }

    public required List<ParcelPoint> LatLngs { get; set; }
  }

  public class ParcelPoint
  {
    [Key]
    public long Id { get; set; }
    public double Lat { get; set; }
    public double Lng { get; set; }
    public short ParcelId { get; set; }
    public Parcel? Parcel { get; set; }
  }
}
