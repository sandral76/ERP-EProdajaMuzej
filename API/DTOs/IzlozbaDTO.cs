using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class IzlozbaDTO
    {
    public int IzlozbaId { get; set; }

    public string Umetnik { get; set; } = null!;

    public string NazivIzlozbe { get; set; } = null!;
    }
}