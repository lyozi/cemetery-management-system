using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.DTOs;
using Domain.Models;
using Domain.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebAPI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ParcelsController : ControllerBase
  {
    private readonly IParcelsService _parcelsService;
    private readonly ILogger<ParcelsController> _log;

    public ParcelsController(IParcelsService parcelsService, ILogger<ParcelsController> log)
    {
      _parcelsService = parcelsService;
      _log = log;
    }

    // GET: api/Parcels
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Parcel>>> GetParcels()
    {
      var parcels = await _parcelsService.GetAllAsync();
      return Ok(parcels);
    }

    // GET: api/Parcels/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Parcel>> GetParcel(short id)
    {
      var parcel = await _parcelsService.GetByIdAsync(id);
      if (parcel == null)
      {
        return NotFound();
      }
      return Ok(parcel);
    }

    // POST: api/Parcels  (upsert: create if missing, replace polygon + reassign graves if existing)
    [HttpPost]
    public async Task<ActionResult<ParcelAssignmentResultDTO>> UpsertParcel([FromBody] ParcelUpsertDTO dto)
    {
      if (dto == null) return BadRequest("Missing parcel payload.");
      if (dto.Id <= 0) return BadRequest("Parcel Id must be a positive number.");

      try
      {
        var result = await _parcelsService.UpsertAndAssignAsync(dto.Id, dto.Name, dto.LatLngs);
        return Ok(result);
      }
      catch (ArgumentException ex)
      {
        return BadRequest(ex.Message);
      }
      catch (Exception ex)
      {
        _log.LogError(ex, "Parcel upsert failed for id={Id}", dto.Id);
        return StatusCode(500, new { error = ex.Message, type = ex.GetType().Name, stack = ex.ToString() });
      }
    }

    // PUT: api/Parcels/5
    [HttpPut("{id}")]
    public async Task<ActionResult<ParcelAssignmentResultDTO>> UpdateParcel(short id, [FromBody] ParcelUpsertDTO dto)
    {
      if (dto == null) return BadRequest("Missing parcel payload.");
      if (id != dto.Id) return BadRequest("Route id and body id must match.");

      try
      {
        var result = await _parcelsService.UpsertAndAssignAsync(dto.Id, dto.Name, dto.LatLngs);
        return Ok(result);
      }
      catch (ArgumentException ex)
      {
        return BadRequest(ex.Message);
      }
      catch (Exception ex)
      {
        _log.LogError(ex, "Parcel upsert failed for id={Id}", dto.Id);
        return StatusCode(500, new { error = ex.Message, type = ex.GetType().Name, stack = ex.ToString() });
      }
    }

    // DELETE: api/Parcels/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteParcel(short id)
    {
      var deleted = await _parcelsService.DeleteAsync(id);
      if (!deleted) return NotFound();
      return NoContent();
    }
  }
}
