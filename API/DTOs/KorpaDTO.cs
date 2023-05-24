namespace API.DTOs
{
    public class KorpaDTO
    {
        public int KorpaId { get; set; }

        public decimal BrojUlaznica { get; set; }

        public decimal UkupanIznos { get; set; }
        public virtual ICollection<StavkaPorudzbineDTO>? StavkaPorudzbines { get; set; }
        //public string ClientSecret { get; set; }
        //public string PaymentIntendId { get; set; }
    }
}