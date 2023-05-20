using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class StavkaPorudzbineDTO
    {
        public int StavkaPorudzbineId { get; set; }

        public decimal CenaStavka { get; set; }

        public decimal? PopustStavka { get; set; }

        public int UlaznicaId { get; set; }

        public int KorpaId { get; set; }

        public virtual string DatumKreiranja { get; set; } = null!;

    }
}