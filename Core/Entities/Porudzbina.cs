namespace Core.Entities;

public partial class Porudzbina
{
         
    public int PorudzbinaId { get; set; }

    public DateTime? DatumKreiranja { get; set; }

    public string StatusPorudzbine { get; set; } = null!;

    public decimal IznosPorudzbine { get; set; }

    public decimal? PopustNaPorudzbinu { get; set; }

    public DateTime? DatumAzuriranja { get; set; }

    public int? DostavaId { get; set; }

    public int KorisnikId { get; set; }

    public virtual DetaljiPorudzbine? Dostava { get; set; }

    public virtual Korisnik Korisnik { get; set; } = null!;

    public virtual Placanje? Placanje { get; set; }

    public virtual ICollection<StavkaPorudzbine> StavkaPorudzbines { get; set;}
    public List<StavkaPorudzbine> Stavke { get; }
}
