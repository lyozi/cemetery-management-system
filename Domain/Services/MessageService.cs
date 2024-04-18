using Domain.Models;
using Domain.RepositoryInterfaces;
using Domain.ServiceInterfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;

        public MessageService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task<IEnumerable<Message>> GetMessagesAsync()
        {
            return await _messageRepository.GetMessagesAsync();
        }

        public async Task<Message> GetMessageByIdAsync(long id)
        {
            return await _messageRepository.GetMessageByIdAsync(id);
        }

        public async Task<Message> CreateMessageAsync(Message message)
        {
            return await _messageRepository.CreateMessageAsync(message);
        }

        public async Task<Message> UpdateMessageAsync(Message message)
        {
            return await _messageRepository.UpdateMessageAsync(message);
        }

        public async Task<bool> DeleteMessageAsync(long id)
        {
            return await _messageRepository.DeleteMessageAsync(id);
        }
    }
}
