using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Models;
using Infrastructure.Context;
using Infrastructure.GraveRepo;

namespace WebAPI.Controllers
{
    [Route("api/Graves")]
    [ApiController]
    public class GravesController : ControllerBase
    {
        private readonly IGraveRepository graveRepository;

        public GravesController(IGraveRepository graveRepository)
        {
            this.graveRepository = graveRepository;
        }

        // GET: api/Graves
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Grave>>> GetGraveItems()
        {
            var graveItems = graveRepository.GetGraves();
            return Ok(graveItems);
        }

        // GET: api/Graves/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Grave>> GetGrave(long id)
        {
            var grave = graveRepository.GetGraveByID(id);

            if (grave == null)
            {
                return NotFound();
            }

            return Ok(grave);
        }

        // PUT: api/Graves/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGrave(long id, Grave grave)
        {
            if (id != grave.Id)
            {
                return BadRequest();
            }

            try
            {
                graveRepository.UpdateGrave(grave);
                graveRepository.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!graveRepository.GraveExists(id))
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

        // POST: api/Graves
        [HttpPost]
        public async Task<ActionResult<Grave>> PostGrave(Grave grave)
        {
            graveRepository.InsertGrave(grave);
            graveRepository.Save();

            return CreatedAtAction(nameof(GetGrave), new { id = grave.Id }, grave);
        }

        // DELETE: api/Graves/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGrave(long id)
        {
            var grave = graveRepository.GetGraveByID(id);
            if (grave == null)
            {
                return NotFound();
            }

            graveRepository.DeleteGrave(id);
            graveRepository.Save();

            return NoContent();
        }

        // DELETE: api/Graves
        [HttpDelete]
        public async Task<IActionResult> DeleteGraves()
        {
            var allGraveItems = graveRepository.GetGraves();
            foreach (var grave in allGraveItems)
            {
                graveRepository.DeleteGrave(grave.Id);
            }
            graveRepository.Save();

            return NoContent();
        }
    }
}
