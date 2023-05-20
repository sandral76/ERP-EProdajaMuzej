using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class KorisnikDostavaDTO
    {
        public string Email { get; set; } = null!;
        public string Ime { get; set; } = null!;
        public string Prezime { get; set; } = null!;
        public string? BrojTel { get; set; } = null;

    }
}