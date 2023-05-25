using Core.Specification;

namespace API.DTOs
{
    public class UlaznicaDTO
    {
    
        public int UlaznicaId { get; set; }

        public decimal? CenaUlaznice { get; set; }

        public string  Izlozba { get; set; } = null!;
        public string? Slika { get; set; } 
        
        public bool Dostupna { get; set; }

    }
}