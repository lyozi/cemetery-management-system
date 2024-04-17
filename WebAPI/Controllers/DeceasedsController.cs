using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Models;
using Infrastructure.Context;
using WebAPI.DTOs;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.DeceasedRepo;

namespace WebAPI.Controllers
{
    [Route("api/Deceased")]
    [ApiController]
    public class DeceasedsController : ControllerBase
    {
        private IDeceasedRepository deceasedRepository;

        public DeceasedsController(IDeceasedRepository deceasedRepository)
        {
            this.deceasedRepository = deceasedRepository;
        }

        // GET: api/Deceaseds
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Deceased>>> GetDeceasedItems()
        {
            var deceasedItems = deceasedRepository.GetDeceaseds();
            return Ok(deceasedItems);
        }

        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<Deceased>>> SearchDeceasedItems(string? name, int? birthYearAfter, int? deceaseYearBefore, string? orderBy)
        {
            var query = deceasedRepository.GetDeceaseds();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(d => d.Name.ToUpper().Contains(name.ToUpper()));
            }

            if (birthYearAfter.HasValue && birthYearAfter > 0)
            {
                query = query.Where(d => d.DateOfBirth.Year >= birthYearAfter);
            }

            if (deceaseYearBefore.HasValue && deceaseYearBefore > 0)
            {
                query = query.Where(d => d.DateOfDeath.Year <= deceaseYearBefore);
            }

            if (!string.IsNullOrEmpty(orderBy))
            {
                switch (orderBy.ToLower())
                {
                    case "name_asc":
                        query = query.OrderBy(d => d.Name);
                        break;
                    case "name_desc":
                        query = query.OrderByDescending(d => d.Name);
                        break;
                    case "dateofdeath_asc":
                        query = query.OrderBy(d => d.DateOfDeath);
                        break;
                    case "dateofdeath_desc":
                        query = query.OrderByDescending(d => d.DateOfDeath);
                        break;
                    case "dateofbirth_asc":
                        query = query.OrderBy(d => d.DateOfBirth);
                        break;
                    case "dateofbirth_desc":
                        query = query.OrderByDescending(d => d.DateOfBirth);
                        break;
                    default:
                        break;
                }
            }
            return Ok(query.ToList());
        }


        [HttpGet("DeceasedsMessages/{id}")]
        public async Task<ActionResult<DeceasedsMessagesDTO>> GetDeceasedMessagesDTO(long id)
        {
            var deceased = await deceasedRepository.GetDeceasedWithMessagesByID(id);

            if (deceased == null)
            {
                return NotFound();
            }

            var dto = DeceasedsMessagesDTO.FromDeceased(deceased);
            return Ok(dto);
        }



        // GET: api/Deceaseds/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Deceased>> GetDeceased(long id)
        {
            var deceased = deceasedRepository.GetDeceasedByID(id);

            if (deceased == null)
            {
                return NotFound();
            }

            return deceased;
        }

        [HttpPut("AddMessage/{id}")]
        public async Task<IActionResult> AddMessage(long id, Message message)
        {
            var deceased = await deceasedRepository.GetDeceasedWithMessagesByID(id);

            if (deceased == null)
            {
                return NotFound();
            }
            else
            {
                if(deceased.MessageList == null)
                {
                    deceased.MessageList = new List<Message>();
                }

                deceased.MessageList.Add(message);

                deceasedRepository.Save();

                return Ok(message);
            }
        }


        // PUT: api/Deceaseds/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDeceased(long id, Deceased deceased)
        {
            if (id != deceased.Id)
            {
                return BadRequest();
            }

            try
            {
                deceasedRepository.UpdateDeceased(deceased);
                deceasedRepository.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (deceasedRepository.DeceasedExists(id))
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


        // POST: api/Deceaseds
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Deceased>> PostDeceased(Deceased deceased)
        {
            deceasedRepository.InsertDeceased(deceased);
            deceasedRepository.Save();

            return CreatedAtAction(nameof(GetDeceased), new { id = deceased.Id }, deceased);
        }

        // DELETE: api/Deceaseds/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeceased(long id)
        {
            var deceased = deceasedRepository.GetDeceasedByID(id);

            if (deceased == null)
            {
                return NotFound();
            }

            deceasedRepository.DeleteDeceased(id);
            deceasedRepository.Save();

            return NoContent();
        }
    }
}
