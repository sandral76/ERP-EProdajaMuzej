namespace Core.Entities;

public partial class Korpa
{
    public Korpa()
    {
        
    }
    public Korpa(int korpaId)
    {
        KorpaId = korpaId;
    }
    public int KorpaId { get; set; }
    public decimal BrojUlaznica { get; set; }

    public decimal UkupanIznos { get; set; }

    public virtual ICollection<StavkaPorudzbine> StavkaPorudzbines { get;set;}
}
