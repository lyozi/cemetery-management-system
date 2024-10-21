using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Message
    {
        public long Id { get; set; }
        public short ItemType { get; set; }
        public string Author { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public DateTime DateOfCreation { get; set; }
        public long DeceasedId { get; set; }
    }
}
