namespace API.DTOs
{
    public class StavkaPorudzbineDTO
    {
        public int StavkaPorudzbineId { get; set; }

        public decimal CenaStavka { get; set; }

        public decimal? PopustStavka { get; set; }

        public int UlaznicaId { get; set; }

        public int KorpaId { get; set; }

        public virtual DateTime? DatumKreiranja { get; set; } = null!;

    }
}