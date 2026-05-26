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
  public class TablesController : ControllerBase
  {
    private readonly ITablesService _tablesService;
    private readonly ILogger<TablesController> _log;

    public TablesController(ITablesService tablesService, ILogger<TablesController> log)
    {
      _tablesService = tablesService;
      _log = log;
    }

    // GET: api/Tables
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Table>>> GetTables()
    {
      var tables = await _tablesService.GetAllAsync();
      return Ok(tables);
    }

    // GET: api/Tables/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Table>> GetTable(short id)
    {
      var table = await _tablesService.GetByIdAsync(id);
      if (table == null)
      {
        return NotFound();
      }
      return Ok(table);
    }

    // POST: api/Tables  (upsert: create if missing, replace polygon + reassign graves if existing)
    [HttpPost]
    public async Task<ActionResult<TableAssignmentResultDTO>> UpsertTable([FromBody] TableUpsertDTO dto)
    {
      if (dto == null) return BadRequest("Missing table payload.");
      if (dto.Id <= 0) return BadRequest("Table Id must be a positive number.");

      try
      {
        var result = await _tablesService.UpsertAndAssignAsync(dto.Id, dto.Name, dto.LatLngs);
        return Ok(result);
      }
      catch (ArgumentException ex)
      {
        return BadRequest(ex.Message);
      }
      catch (Exception ex)
      {
        _log.LogError(ex, "Table upsert failed for id={Id}", dto.Id);
        return StatusCode(500, new { error = ex.Message, type = ex.GetType().Name, stack = ex.ToString() });
      }
    }

    // PUT: api/Tables/5
    [HttpPut("{id}")]
    public async Task<ActionResult<TableAssignmentResultDTO>> UpdateTable(short id, [FromBody] TableUpsertDTO dto)
    {
      if (dto == null) return BadRequest("Missing table payload.");
      if (id != dto.Id) return BadRequest("Route id and body id must match.");

      try
      {
        var result = await _tablesService.UpsertAndAssignAsync(dto.Id, dto.Name, dto.LatLngs);
        return Ok(result);
      }
      catch (ArgumentException ex)
      {
        return BadRequest(ex.Message);
      }
      catch (Exception ex)
      {
        _log.LogError(ex, "Table upsert failed for id={Id}", dto.Id);
        return StatusCode(500, new { error = ex.Message, type = ex.GetType().Name, stack = ex.ToString() });
      }
    }

    // DELETE: api/Tables/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTable(short id)
    {
      var deleted = await _tablesService.DeleteAsync(id);
      if (!deleted) return NotFound();
      return NoContent();
    }
  }
}
