using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class KorisnikDTO
    {
        public int KorisnikId { get; set; }
        [Required(ErrorMessage="Morate uneti korisničko ime.")]
        [RegularExpression(@"^.{3,100}$",ErrorMessage="Korisničko ime mora imati barem 3 znaka.")]
        public string KorisnickoIme { get; set; } = null!;

        public string Ime { get; set; } = null!;

        public string Prezime { get; set; } = null!;

        public string  TipKorisnika { get; set; } = null!;
        public virtual ICollection<PorudzbinaDTO>? Porudzbinas { get;set; }
    }
}