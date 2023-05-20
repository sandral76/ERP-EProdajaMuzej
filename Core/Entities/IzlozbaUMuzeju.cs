using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

public partial class IzlozbaUMuzeju
{
    public int MuzejId { get; set; }

    public int IzlozbaId { get; set; }

    public DateTime? DatumOdrzavanja { get; set; }
    [Required(ErrorMessage="Morate uneti galeriju u kojoj se održava izložba.")]
    public string Galerija { get; set; } = null!;

    public virtual Izlozba Izlozba { get; set; } = null!;

    public virtual Muzej Muzej { get; set; } = null!;
}
