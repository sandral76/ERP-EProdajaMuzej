namespace API.DTOs
{
    public class IzlozbaUMuzejuDTO
    {
        public DateTime? DatumOdrzavanja { get; set; }

        public string Galerija { get; set; } = null!;

        public virtual string Izlozba { get; set; } = null!;

        public virtual string Muzej { get; set; } = null!;
    }
}