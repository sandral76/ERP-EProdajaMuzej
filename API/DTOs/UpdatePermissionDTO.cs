using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class UpdatePermissionDTO
    {   
        [Required(ErrorMessage="Morate uneti korisničko ime.")]
        public string KorisnickoIme { get; set; } = null!;

    }
}