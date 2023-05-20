using System;
using System.Collections.Generic;

namespace Core.Entities;

public partial class StavkaPorudzbine
{
    public int StavkaPorudzbineId { get; set; }

    public decimal CenaStavka { get; set; }

    public decimal? PopustStavka { get; set; }

    public int UlaznicaId { get; set; }

    public int KorpaId { get; set; }

    public int PorudzbinaId { get; set; }
    
    //public int Kolicina {get;set;}

    public virtual Korpa Korpa { get; set; } = null!;

    public virtual Porudzbina Porudzbina { get; set; } = null!;

    public virtual Ulaznica Ulaznica { get; set; } = null!;
}
