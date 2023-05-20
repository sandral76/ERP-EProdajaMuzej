using System;
using System.Collections.Generic;

namespace Core.Entities;

public partial class Placanje
{
    public int PlacanjeId { get; set; }

    public DateTime? DatumPlacanja { get; set; }

    public string? TipPlacanja { get; set; }

    public decimal? BrojRacuna { get; set; }

    public string VlasnikKartice { get; set; } = null!;

    public int PorudzbinaId { get; set; }

    public virtual Porudzbina Porudzbina { get; set; } = null!;
}
