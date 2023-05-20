using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class DetaljiPorudzbineDTO
    {
        public string EmailDostave { get; set; } = null!;

        public string? Ime { get; set; }

        public string? Prezime { get; set; }
        public string? KontaktTelefon { get; set; } = null;

    }
}