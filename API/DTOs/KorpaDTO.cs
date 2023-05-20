using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Specification;

namespace API.DTOs
{
    public class KorpaDTO
    {


        public int KorpaId { get; set; }

        public decimal BrojUlaznica { get; set; }

        public decimal UkupanIznos { get; set; }
        public virtual ICollection<StavkaPorudzbineDTO>? StavkaPorudzbines { get; set; }
    }
}