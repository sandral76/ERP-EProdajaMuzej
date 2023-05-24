namespace Core.Entities
{
    public class StavkaPorudzbineDTO2
    {
        public int StavkaPorudzbineId { get; set; }

        public decimal CenaStavka { get; set; }

        public decimal? PopustStavka { get; set; }

        public int UlaznicaId { get; set; }

        public int KorpaId { get; set; }

        public virtual DateTime? DatumKreiranja { get; set; } = null!;

    }
}