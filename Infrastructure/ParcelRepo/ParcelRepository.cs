using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models;
using Domain.RepositoryInterfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ParcelRepo
{
  public class ParcelRepository : IParcelRepository
  {
    private readonly DatabaseContext _context;

    public ParcelRepository(DatabaseContext context)
    {
      _context = context;
    }

    public async Task<List<Parcel>> GetAllAsync()
    {
      var parcels = await _context.Parcels
        .Include(p => p.LatLngs)
        .AsNoTracking()
        .ToListAsync();

      foreach (var p in parcels)
      {
        p.LatLngs = p.LatLngs.OrderBy(pt => pt.Id).ToList();
      }
      return parcels;
    }

    public async Task<Parcel?> GetByIdAsync(short id)
    {
      var parcel = await _context.Parcels
        .Include(p => p.LatLngs)
        .FirstOrDefaultAsync(p => p.Id == id);

      if (parcel != null)
      {
        parcel.LatLngs = parcel.LatLngs.OrderBy(pt => pt.Id).ToList();
      }
      return parcel;
    }

    public async Task<Parcel> UpsertAsync(short id, string? name, List<ParcelPoint> latLngs)
    {
      var existing = await _context.Parcels
        .FirstOrDefaultAsync(p => p.Id == id);

      if (existing == null)
      {
        var parcel = new Parcel
        {
          Id = id,
          Name = name,
          LatLngs = latLngs,
        };
        _context.Parcels.Add(parcel);
        await _context.SaveChangesAsync();
        return parcel;
      }

      existing.Name = name;

      // Drop old points via direct SQL — avoids tracking/concurrency snags on
      // batched DELETEs and is a single round-trip.
      await _context.ParcelPoints
        .Where(pt => pt.ParcelId == id)
        .ExecuteDeleteAsync();

      foreach (var pt in latLngs)
      {
        pt.Id = 0;
        pt.ParcelId = existing.Id;
        _context.ParcelPoints.Add(pt);
      }
      await _context.SaveChangesAsync();

      var reloaded = await _context.Parcels
        .Include(p => p.LatLngs)
        .AsNoTracking()
        .FirstAsync(p => p.Id == id);
      reloaded.LatLngs = reloaded.LatLngs.OrderBy(pt => pt.Id).ToList();
      return reloaded;
    }

    public async Task<bool> DeleteAsync(short id)
    {
      var parcel = await _context.Parcels
        .Include(p => p.LatLngs)
        .FirstOrDefaultAsync(p => p.Id == id);

      if (parcel == null) return false;

      _context.Parcels.Remove(parcel);
      await _context.SaveChangesAsync();
      return true;
    }

    public async Task<List<Grave>> GetGravesWithPolygonsAsync()
    {
      return await _context.GraveItems
        .Include(g => g.GraveUIPolygon)
        .ThenInclude(p => p!.LatLngs)
        .Where(g => g.GraveUIPolygon != null)
        .ToListAsync();
    }

    public void MarkGraveModified(Grave grave)
    {
      _context.Entry(grave).Property(g => g.Parcel).IsModified = true;
    }

    public Task SaveAsync()
    {
      return _context.SaveChangesAsync();
    }
  }
}
