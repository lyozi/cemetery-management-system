using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
  public class Table
  {
    [Key]
    public short Id { get; set; }

    public string? Name { get; set; }

    public required List<TablePoint> LatLngs { get; set; }
  }

  public class TablePoint
  {
    [Key]
    public long Id { get; set; }
    public double Lat { get; set; }
    public double Lng { get; set; }
    public short TableId { get; set; }
    public Table? Table { get; set; }
  }
}
