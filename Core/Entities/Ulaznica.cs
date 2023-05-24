using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

public partial class Ulaznica 
{
    [Key]
    public readonly int UlaznicaId;

    public decimal CenaUlaznice { get; set; }

    public int IzlozbaId { get; set; }

    public bool Dostupna {get; set;}

    public virtual Izlozba Izlozba { get; set; } = null!;

    //public virtual StavkaPorudzbine? StavkaPorudzbine { get; set; }
    
}
