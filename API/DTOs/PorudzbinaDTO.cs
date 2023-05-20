using Core.Entities;

namespace API.DTOs
{
    public class PorudzbinaDTO
    {
        public int PorudzbinaId { get; set; }

        public DateTime? DatumKreiranja { get; set; }

        public string StatusPorudzbine { get; set; } = null!;

        public decimal IznosPorudzbine { get; set; }

        public decimal? PopustNaPorudzbinu { get; set; }

        public DateTime? DatumAzuriranja { get; set; }

        public virtual string? Dostava { get; set; }

        public virtual string Korisnik { get; set; } = null!;
        public virtual ICollection<StavkaPorudzbineDTO>? StavkaPorudzbines { get; set; }

    }
}