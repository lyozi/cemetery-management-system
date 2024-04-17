using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.MessageRepo;

namespace WebAPI.Controllers
{
    [Route("api/Messages")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageRepository _messageRepository;

        public MessagesController(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        // GET: api/Messages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessageItems()
        {
            var messages = await _messageRepository.GetMessagesAsync();
            return Ok(messages);
        }

        // GET: api/Messages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Message>> GetMessage(long id)
        {
            var message = await _messageRepository.GetMessageByIdAsync(id);
            if (message == null)
            {
                return NotFound();
            }

            return message;
        }

        // PUT: api/Messages/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMessage(long id, Message message)
        {
            if (id != message.Id)
            {
                return BadRequest();
            }

            var updatedMessage = await _messageRepository.UpdateMessageAsync(message);
            return Ok(updatedMessage);
        }

        // POST: api/Messages
        [HttpPost]
        public async Task<ActionResult<Message>> PostMessage(Message message)
        {
            var createdMessage = await _messageRepository.CreateMessageAsync(message);
            return CreatedAtAction(nameof(GetMessage), new { id = createdMessage.Id }, createdMessage);
        }

        // DELETE: api/Messages/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(long id)
        {
            var result = await _messageRepository.DeleteMessageAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
