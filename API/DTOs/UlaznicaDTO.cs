using Core.Specification;

namespace API.DTOs
{
    public class UlaznicaDTO
    {
    
        public int UlaznicaId { get; set; }

        public decimal? CenaUlaznice { get; set; }

        public string  Izlozba { get; set; } = null!;

        public bool Dostupna { get; set; }

    }
}