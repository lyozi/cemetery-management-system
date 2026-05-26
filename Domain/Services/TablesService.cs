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
  public class TablesService : ITablesService
  {
    private const short UnassignedTable = 0;
    private static readonly GeometryFactory GeoFactory = new();

    private readonly ITableRepository _tableRepository;

    public TablesService(ITableRepository tableRepository)
    {
      _tableRepository = tableRepository;
    }

    public Task<List<Table>> GetAllAsync() => _tableRepository.GetAllAsync();

    public Task<Table?> GetByIdAsync(short id) => _tableRepository.GetByIdAsync(id);

    public async Task<TableAssignmentResultDTO> UpsertAndAssignAsync(
      short id,
      string? name,
      List<TablePointDTO> latLngs)
    {
      if (latLngs == null || latLngs.Count < 3)
      {
        throw new ArgumentException("A table polygon must have at least 3 points.", nameof(latLngs));
      }

      var entityPoints = latLngs
        .Select(p => new TablePoint { Lat = p.Lat, Lng = p.Lng })
        .ToList();

      var table = await _tableRepository.UpsertAsync(id, name, entityPoints);
      var tablePolygon = BuildPolygon(latLngs.Select(p => (p.Lat, p.Lng)));

      var graves = await _tableRepository.GetGravesWithPolygonsAsync();
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

        var isInside = tablePolygon.Contains(graveGeom.Centroid);
        var wasAssigned = grave.Table == id;

        if (isInside && !wasAssigned)
        {
          grave.Table = id;
          _tableRepository.MarkGraveModified(grave);
          assigned++;
        }
        else if (!isInside && wasAssigned)
        {
          grave.Table = UnassignedTable;
          _tableRepository.MarkGraveModified(grave);
          unassigned++;
        }
      }

      if (assigned > 0 || unassigned > 0)
      {
        await _tableRepository.SaveAsync();
      }

      return new TableAssignmentResultDTO
      {
        Table = table,
        AssignedCount = assigned,
        UnassignedCount = unassigned,
      };
    }

    public Task<bool> DeleteAsync(short id)
    {
      return _tableRepository.DeleteAsync(id);
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
