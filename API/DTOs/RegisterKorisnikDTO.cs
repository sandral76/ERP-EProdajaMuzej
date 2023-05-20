using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class RegisterKorisnikDTO
    {
        [Required(ErrorMessage = "Morate uneti korisničko ime.")]
        public string KorisnickoIme { get; set; } = null!;

        [Required(ErrorMessage = "Morate uneti lozinku.")]
        [RegularExpression("^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?!.*\\s).{4,8}$",ErrorMessage ="Lozinka mora sadržati 1 malo slovo, 1 veliko slovo, 1 znak i barem 6 znakova.")]
        public string Lozinka { get; set; } = null!;
        [Required(ErrorMessage = "Morate uneti ime.")]
        public string Ime { get; set; } = null!;
        [Required(ErrorMessage = "Morate uneti prezime.")]
        public string Prezime { get; set; } = null!;
        [Required(ErrorMessage = "Morate uneti broj telefona.")]
        public string BrojTel { get; set; } = null!;
        [Required(ErrorMessage = "Morate uneti email.")]
        [EmailAddress(ErrorMessage = "Uneta email adresa nije u ispravnom formatu.")]
        public string Email { get; set; } = null!;
    }
}