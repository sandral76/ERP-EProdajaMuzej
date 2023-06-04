using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

public partial class Izlozba
{
    public int IzlozbaId { get; set; }
    [Required(ErrorMessage="Morate uneti umetnika.")]
    public string Umetnik { get; set; } = null!;
    [Required(ErrorMessage="Morate uneti naziv izložbe.")]
    public string NazivIzlozbe { get; set; } = null!;
   
    public virtual ICollection<IzlozbaUMuzeju> IzlozbaUMuzejus { get; } = new List<IzlozbaUMuzeju>();

    public virtual ICollection<Ulaznica> Ulaznicas { get; } = new List<Ulaznica>();

}
