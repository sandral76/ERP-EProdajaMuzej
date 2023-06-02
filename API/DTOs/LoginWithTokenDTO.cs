using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class LoginWithTokenDTO

    {
        public int KorisnikId { get; set; }
        [Required(ErrorMessage="Morate uneti korisničko ime.")]
        [RegularExpression(@"^.{3,100}$",ErrorMessage="Korisničko ime mora imati barem 3 znaka.")]
        public string KorisnickoIme { get; set; } = null!;
        [Required(ErrorMessage="Morate uneti lozniku.")]
        [RegularExpression("^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?!.*\\s).{4,8}$",ErrorMessage ="Lozinka mora sadržati 1 malo slovo, 1 veliko slovo, 1 znak i barem 6 znakova.")]
        public string Lozinka { get; set; } = null!;
        public int TipKorisnika { get; set; }
        public string Token {get;set;}
    }
}