using Domain.Models;

namespace Domain.DTOs
{
  public class GravePositionDataDTO
  {
    public List<Point> LatLngs { get; set; } = new List<Point>();
    public char Table { get; set; }
    public short Row { get; set; }
    public short Parcel { get; set; }
    public short StructureType { get; set; }

  }
}
