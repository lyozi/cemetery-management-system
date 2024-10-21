using Domain.Models;

namespace Domain.DTOs
{
    public class DeceasedDataDTO
    {
        public long Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public DateTime DateOfDeath { get; set; }
        public DateTime DateOfBirth { get; set; }
        public ICollection<Message> MessageList { get; set; } = new List<Message>();
        public char GraveTable { get; set; }
        public short GraveRow { get; set; }
        public short GraveParcel { get; set; }
    }
}
