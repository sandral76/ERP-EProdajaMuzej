using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace API.DTOs
{
    public class PorudzbinaToCreateDto
    {
        public int KorpaId { get; set; }
        public virtual DetaljiPorudzbine? Dostava { get; set; }

    }
}