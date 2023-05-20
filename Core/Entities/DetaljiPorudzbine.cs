using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

public partial class DetaljiPorudzbine
{
    public int DostavaId { get; set; }

    [Required(ErrorMessage="Morate uneti email dostave.")]
    public string EmailDostave { get; set; } = null!;

    public string? Ime { get; set; }

    public string? Prezime { get; set; }
    [Required(ErrorMessage="Morate uneti kontakt telefon za dostavu.")]
    public string KontaktTelefon { get; set; } = null!;

    public virtual ICollection<Porudzbina> Porudzbinas { get; } = new List<Porudzbina>();
}
