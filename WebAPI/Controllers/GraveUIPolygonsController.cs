using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Models;
using Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;
using Domain.ServiceInterfaces;
using Domain.Services;
using Domain.DTOs;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GraveUIPolygonsController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly IGravesService _gravesService;

        public GraveUIPolygonsController(DatabaseContext context, IGravesService gravesService)
        {
            _context = context;
            _gravesService = gravesService;
        }

        // GET: api/GraveUIPolygons
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GraveUIPolygon>>> GetGraveUIPolygons()
        {
            try
            {
                var polygons = await _context.GraveUIPolygons
                .Include(polygon => polygon.LatLngs)
                .Include(polygon => polygon.Grave)
                .ThenInclude(g => g.DeceasedList)
                .ToListAsync();

                return polygons;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }


        // GET: api/GraveUIPolygons/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GraveUIPolygon>> GetGraveUIPolygon(long id)
        {
            var graveUIPolygon = await _context.GraveUIPolygons.FindAsync(id);

            if (graveUIPolygon == null)
            {
                return NotFound();
            }

            return graveUIPolygon;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutGraveUIPolygon(long id, GraveUIPolygon graveUIPolygon)
        {
            if (id != graveUIPolygon.Id)
            {
                return BadRequest("Az új és a meglévő entitás azonosítója nem egyezik meg.");
            }

            if (graveUIPolygon.LatLngs == null || graveUIPolygon.LatLngs.Count != 4)
            {
                return BadRequest("A GraveUIPolygon objektumnak pontosan 4 ponttal kell rendelkeznie.");
            }

            _context.Entry(graveUIPolygon).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GraveUIPolygonExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<GraveUIPolygon>> PostGraveUIPolygonAndGrave(GravePositionDataDTO dto)
        {
            var grave = _gravesService.GetOrCreateGrave(dto.Table, dto.Row, dto.Parcel);

            var graveUIPolygon = new GraveUIPolygon
            {
                LatLngs = dto.LatLngs,
                GraveId = grave.Id
            };

            _context.GraveUIPolygons.Add(graveUIPolygon);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetGraveUIPolygon), new { id = graveUIPolygon.Id }, graveUIPolygon);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGraveUIPolygon(long id)
        {
            var graveUIPolygon = await _context.GraveUIPolygons.FindAsync(id);
            if (graveUIPolygon == null)
            {
                return NotFound();
            }

            _context.GraveUIPolygons.Remove(graveUIPolygon);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteGraveUIPolygons()
        {
            try
            {
                await _context.Points.Where(p => p.GraveUIPolygonId != null).ForEachAsync(point =>
                {
                    _context.Points.Remove(point);
                });

                _context.GraveUIPolygons.RemoveRange(_context.GraveUIPolygons);

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        private bool GraveUIPolygonExists(long id)
        {
            return _context.GraveUIPolygons.Any(e => e.Id == id);
        }
    }
}
