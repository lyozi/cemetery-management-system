using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Domain.ServiceInterfaces;
using Domain.DTOs;

namespace WebAPI.Controllers
{
    [Route("api/Deceased")]
    [ApiController]
    public class DeceasedsController : ControllerBase
    {
        private readonly IDeceasedService _deceasedService;

        public DeceasedsController(IDeceasedService deceasedService)
        {
            _deceasedService = deceasedService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Deceased>> GetDeceasedItems()
        {
            var deceasedItems = _deceasedService.GetDeceaseds();
            return Ok(deceasedItems);
        }

        [HttpGet("Search")]
        public ActionResult<IEnumerable<Deceased>> SearchDeceasedItems(string? name, int? birthYearAfter, int? deceaseYearBefore, string? orderBy)
        {
            var deceasedItems = _deceasedService.SearchDeceaseds(name, birthYearAfter, deceaseYearBefore, orderBy);
            return Ok(deceasedItems);
        }

        [HttpGet("DeceasedsMessages/{id}")]
        public async Task<ActionResult<DeceasedsMessagesDTO>> GetDeceasedMessagesDTO(long id)
        {
            var deceased = await _deceasedService.GetDeceasedWithMessagesByID(id);

            if (deceased == null)
            {
                return NotFound();
            }

            var dto = DeceasedsMessagesDTO.FromDeceased(deceased);
            return Ok(dto);
        }

        [HttpGet("{id}")]
        public ActionResult<Deceased> GetDeceased(long id)
        {
            var deceased = _deceasedService.GetDeceasedByID(id);

            if (deceased == null)
            {
                return NotFound();
            }

            return deceased;
        }

        [HttpPut("AddMessage/{id}")]
        public IActionResult AddMessage(long id, Message message)
        {
            _deceasedService.AddMessageToDeceased(id, message);
            return Ok(message);
        }

        [Authorize(Policy = "Manager")]
        [HttpPut("{id}")]
        public IActionResult PutDeceased(long id, Deceased deceased)
        {
            if (id != deceased.Id)
            {
                return BadRequest();
            }

            try
            {
                _deceasedService.UpdateDeceased(deceased);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_deceasedService.DeceasedExists(id))
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

        [Authorize(Policy = "Manager")]
        [HttpPost]
        public ActionResult<Deceased> PostDeceased(DeceasedDataDTO deceasedData)
        {
            var deceased = _deceasedService.CreateDeceased(deceasedData);
            return CreatedAtAction(nameof(GetDeceased), new { id = deceased.Id }, deceased);
        }

        [Authorize(Policy = "Manager")]
        [HttpPost("BulkAdd")]
        public ActionResult<IEnumerable<Deceased>> PostDeceaseds(IEnumerable<DeceasedDataDTO> deceasedDataList)
        {
            var deceasedList = _deceasedService.CreateDeceaseds(deceasedDataList);
            return CreatedAtAction(nameof(GetDeceasedItems), deceasedList);
        }

        [Authorize(Policy = "Manager")]
        [HttpDelete("{id}")]
        public IActionResult DeleteDeceased(long id)
        {
            var deceased = _deceasedService.GetDeceasedByID(id);

            if (deceased == null)
            {
                return NotFound();
            }

            _deceasedService.DeleteDeceased(id);
            return NoContent();
        }
    }
}
