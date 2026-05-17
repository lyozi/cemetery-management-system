using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.DTOs;
using Domain.Models;
using Domain.RepositoryInterfaces;
using Domain.ServiceInterfaces;
using NetTopologySuite.Geometries;

namespace Domain.Services
{
  public class ParcelsService : IParcelsService
  {
    private const short UnassignedParcel = 0;
    private static readonly GeometryFactory GeoFactory = new();

    private readonly IParcelRepository _parcelRepository;

    public ParcelsService(IParcelRepository parcelRepository)
    {
      _parcelRepository = parcelRepository;
    }

    public Task<List<Parcel>> GetAllAsync() => _parcelRepository.GetAllAsync();

    public Task<Parcel?> GetByIdAsync(short id) => _parcelRepository.GetByIdAsync(id);

    public async Task<ParcelAssignmentResultDTO> UpsertAndAssignAsync(
      short id,
      string? name,
      List<ParcelPointDTO> latLngs)
    {
      if (latLngs == null || latLngs.Count < 3)
      {
        throw new ArgumentException("A parcel polygon must have at least 3 points.", nameof(latLngs));
      }

      var entityPoints = latLngs
        .Select(p => new ParcelPoint { Lat = p.Lat, Lng = p.Lng })
        .ToList();

      var parcel = await _parcelRepository.UpsertAsync(id, name, entityPoints);
      var parcelPolygon = BuildPolygon(latLngs.Select(p => (p.Lat, p.Lng)));

      var graves = await _parcelRepository.GetGravesWithPolygonsAsync();
      int assigned = 0;
      int unassigned = 0;

      foreach (var grave in graves)
      {
        if (grave.GraveUIPolygon?.LatLngs == null || grave.GraveUIPolygon.LatLngs.Count < 3)
        {
          continue;
        }

        var graveGeom = TryBuildPolygon(grave.GraveUIPolygon.LatLngs.Select(p => (p.Lat, p.Lng)));
        if (graveGeom == null) continue;

        var isInside = parcelPolygon.Contains(graveGeom.Centroid);
        var wasAssigned = grave.Parcel == id;

        if (isInside && !wasAssigned)
        {
          grave.Parcel = id;
          _parcelRepository.MarkGraveModified(grave);
          assigned++;
        }
        else if (!isInside && wasAssigned)
        {
          grave.Parcel = UnassignedParcel;
          _parcelRepository.MarkGraveModified(grave);
          unassigned++;
        }
      }

      if (assigned > 0 || unassigned > 0)
      {
        await _parcelRepository.SaveAsync();
      }

      return new ParcelAssignmentResultDTO
      {
        Parcel = parcel,
        AssignedCount = assigned,
        UnassignedCount = unassigned,
      };
    }

    public Task<bool> DeleteAsync(short id)
    {
      return _parcelRepository.DeleteAsync(id);
    }

    private static Polygon BuildPolygon(IEnumerable<(double Lat, double Lng)> points)
    {
      var coords = points
        .Select(p => new Coordinate(p.Lng, p.Lat))
        .ToList();

      if (coords.Count < 3)
      {
        throw new ArgumentException("Polygon requires at least 3 points.");
      }

      if (!coords[0].Equals2D(coords[^1]))
      {
        coords.Add(new Coordinate(coords[0].X, coords[0].Y));
      }

      var ring = GeoFactory.CreateLinearRing(coords.ToArray());
      return GeoFactory.CreatePolygon(ring);
    }

    private static Polygon? TryBuildPolygon(IEnumerable<(double Lat, double Lng)> points)
    {
      try
      {
        return BuildPolygon(points);
      }
      catch
      {
        return null;
      }
    }
  }
}
