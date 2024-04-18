using Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Context;
using Domain.RepositoryInterfaces;

namespace Infrastructure.MessageRepo
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DatabaseContext _context;

        public MessageRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Message>> GetMessagesAsync()
        {
            return await _context.MessageItems.ToListAsync();
        }

        public async Task<Message> GetMessageByIdAsync(long id)
        {
            return await _context.MessageItems.FindAsync(id);
        }

        public async Task<Message> CreateMessageAsync(Message message)
        {
            _context.MessageItems.Add(message);
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<Message> UpdateMessageAsync(Message message)
        {
            _context.Entry(message).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<bool> DeleteMessageAsync(long id)
        {
            var message = await _context.MessageItems.FindAsync(id);
            if (message == null)
                return false;

            _context.MessageItems.Remove(message);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}