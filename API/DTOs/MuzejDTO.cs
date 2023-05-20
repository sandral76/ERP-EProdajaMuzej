using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class MuzejDTO
    {
        public int MuzejId { get; set; }

        public string Naziv { get; set; } = null!;

        public string Grad { get; set; } = null!;

    }
}