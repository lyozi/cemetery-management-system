using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
  public class GraveUIPolygon
  {
    public long Id { get; set; }
    public short StructureType { get; set; }
    public required List<Point> LatLngs { get; set; }
    public long GraveId { get; set; }
    public Grave? Grave { get; set; }
  }


  public class Point
  {
    public long? Id { get; set; }
    public double Lat { get; set; }
    public double Lng { get; set; }
    public long GraveUIPolygonId { get; set; }
  }
}
