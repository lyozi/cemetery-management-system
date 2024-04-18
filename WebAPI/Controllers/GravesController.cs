using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Domain.Models;
using Domain.Services;
using Domain.ServiceInterfaces;

namespace WebAPI.Controllers
{
    [Route("api/Graves")]
    [ApiController]
    public class GravesController : ControllerBase
    {
        private readonly IGravesService _gravesService;

        public GravesController(IGravesService gravesService)
        {
            _gravesService = gravesService;
        }

        // GET: api/Graves
        [HttpGet]
        public ActionResult<IEnumerable<Grave>> GetGraveItems()
        {
            var graveItems = _gravesService.GetGraves();
            return Ok(graveItems);
        }

        // GET: api/Graves/5
        [HttpGet("{id}")]
        public ActionResult<Grave> GetGrave(long id)
        {
            var grave = _gravesService.GetGraveByID(id);

            if (grave == null)
            {
                return NotFound();
            }

            return Ok(grave);
        }

        // PUT: api/Graves/5
        [HttpPut("{id}")]
        public IActionResult PutGrave(long id, Grave grave)
        {
            if (id != grave.Id)
            {
                return BadRequest();
            }

            try
            {
                _gravesService.UpdateGrave(grave);
            }
            catch (Exception)
            {
                if (!_gravesService.GraveExists(id))
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
        public ActionResult<Grave> PostGrave(Grave grave)
        {
            _gravesService.InsertGrave(grave);

            return CreatedAtAction(nameof(GetGrave), new { id = grave.Id }, grave);
        }

        // DELETE: api/Graves/5
        [HttpDelete("{id}")]
        public IActionResult DeleteGrave(long id)
        {
            var grave = _gravesService.GetGraveByID(id);
            if (grave == null)
            {
                return NotFound();
            }

            _gravesService.DeleteGrave(id);

            return NoContent();
        }

        // DELETE: api/Graves
        [HttpDelete]
        public IActionResult DeleteGraves()
        {
            var allGraveItems = _gravesService.GetGraves();
            foreach (var grave in allGraveItems)
            {
                _gravesService.DeleteGrave(grave.Id);
            }

            return NoContent();
        }
    }
}
