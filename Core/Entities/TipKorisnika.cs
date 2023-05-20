using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

public partial class TipKorisnika
{
    public int TipKorisnikaId { get; set; }
    
    [RegularExpression("Admin|Registrovani korisnik|Super korisnik")]
    public string? Uloga { get; set; }

    public virtual ICollection<Korisnik> Korisniks { get; } = new List<Korisnik>();
}
