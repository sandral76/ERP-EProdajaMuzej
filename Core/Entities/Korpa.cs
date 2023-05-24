namespace Core.Entities;

public partial class Korpa
{
    public int KorpaId { get; set; }
    public decimal BrojUlaznica { get; set; }

    public decimal UkupanIznos { get; set; }

    public virtual ICollection<StavkaPorudzbine> StavkaPorudzbines { get; set; }

    public string ClientSecret { get; set; }
    public string PaymentIntendId { get; set; }

}
