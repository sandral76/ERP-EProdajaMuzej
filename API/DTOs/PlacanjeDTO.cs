using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class PlacanjeDTO
    {
        public int PlacanjeId { get; set; }

        public DateTime? DatumPlacanja { get; set; }

        public string? TipPlacanja { get; set; }

        public virtual string IznosPorudzbine { get; set; } = null!;
    }
}