using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models;
using Domain.RepositoryInterfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.TableRepo
{
  public class TableRepository : ITableRepository
  {
    private readonly DatabaseContext _context;

    public TableRepository(DatabaseContext context)
    {
      _context = context;
    }

    public async Task<List<Table>> GetAllAsync()
    {
      var tables = await _context.Tables
        .Include(t => t.LatLngs)
        .AsNoTracking()
        .ToListAsync();

      foreach (var t in tables)
      {
        t.LatLngs = t.LatLngs.OrderBy(pt => pt.Id).ToList();
      }
      return tables;
    }

    public async Task<Table?> GetByIdAsync(short id)
    {
      var table = await _context.Tables
        .Include(t => t.LatLngs)
        .FirstOrDefaultAsync(t => t.Id == id);

      if (table != null)
      {
        table.LatLngs = table.LatLngs.OrderBy(pt => pt.Id).ToList();
      }
      return table;
    }

    public async Task<Table> UpsertAsync(short id, string? name, List<TablePoint> latLngs)
    {
      var existing = await _context.Tables
        .FirstOrDefaultAsync(t => t.Id == id);

      if (existing == null)
      {
        var table = new Table
        {
          Id = id,
          Name = name,
          LatLngs = latLngs,
        };
        _context.Tables.Add(table);
        await _context.SaveChangesAsync();
        return table;
      }

      existing.Name = name;

      // Drop old points via direct SQL — avoids tracking/concurrency snags on
      // batched DELETEs and is a single round-trip.
      await _context.TablePoints
        .Where(pt => pt.TableId == id)
        .ExecuteDeleteAsync();

      foreach (var pt in latLngs)
      {
        pt.Id = 0;
        pt.TableId = existing.Id;
        _context.TablePoints.Add(pt);
      }
      await _context.SaveChangesAsync();

      var reloaded = await _context.Tables
        .Include(t => t.LatLngs)
        .AsNoTracking()
        .FirstAsync(t => t.Id == id);
      reloaded.LatLngs = reloaded.LatLngs.OrderBy(pt => pt.Id).ToList();
      return reloaded;
    }

    public async Task<bool> DeleteAsync(short id)
    {
      var table = await _context.Tables
        .Include(t => t.LatLngs)
        .FirstOrDefaultAsync(t => t.Id == id);

      if (table == null) return false;

      _context.Tables.Remove(table);
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
      _context.Entry(grave).Property(g => g.Table).IsModified = true;
    }

    public Task SaveAsync()
    {
      return _context.SaveChangesAsync();
    }
  }
}
