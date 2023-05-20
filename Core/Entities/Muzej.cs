using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

public partial class Muzej
{   
    [Key]
    public int MuzejId { get; set; }
    [Required(ErrorMessage="Morate uneti naziv muzeja.")]
    public string Naziv { get; set; } = null!;
    [Required(ErrorMessage="Morate uneti grad.")]
    public string Grad { get; set; } = null!;
    [Required(ErrorMessage="Morate uneti ulicu.")]    
    public string Ulica { get; set; } = null!;

    public string? BrojTelefona { get; set; }
    [Required(ErrorMessage="Morate uneti direktora muzeja.")]
    public string Direktor { get; set; } = null!;

    public string? VebSajt { get; set; }

    public virtual ICollection<IzlozbaUMuzeju> IzlozbaUMuzejus { get; } = new List<IzlozbaUMuzeju>();

    public static implicit operator Muzej(List<string> v)
    {
        throw new NotImplementedException();
    }
}
