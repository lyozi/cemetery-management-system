using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Domain.Models
{
    [Index(nameof(Number), IsUnique = true)]
    public class Grave
    {
        [Key]
        public long Id { get; set; }
        public short Number { get; set; }
        public short Type { get; set; } = 0;

        // Koordináták
        public GraveUIPolygon? GraveUIPolygon { get; set; } = null;

        // public byte[] Image { get; set; } = new byte[0];

        public ICollection<Deceased>? DeceasedList { get; set; } = null;
    }
}