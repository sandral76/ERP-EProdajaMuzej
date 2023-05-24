namespace Core.Entities
{
    public class KorpaDTO2
    {
        public int KorpaId { get; set; }

        public decimal BrojUlaznica { get; set; }

        public decimal UkupanIznos { get; set; }
        public virtual ICollection<StavkaPorudzbineDTO2>? StavkaPorudzbines { get; set; }
        public string ClientSecret { get; set; }
        public string PaymentIntendId { get; set; }
    }
}