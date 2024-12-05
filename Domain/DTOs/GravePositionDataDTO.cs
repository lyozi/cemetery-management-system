using Domain.Models;
using Microsoft.AspNetCore.Http;

namespace Domain.DTOs
{
  public class GravePositionDataDTO
  {
    public string LatLngs { get; set; } = "";
    public char Table { get; set; }
    public short Row { get; set; }
    public short Parcel { get; set; }
    public short StructureType { get; set; }
    public IFormFile? Image { get; set; }
  }

}
