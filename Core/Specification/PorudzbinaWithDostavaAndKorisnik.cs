using Core.Entities;

namespace Core.Specification
{
    public class PorudzbinaWithDostavaAndKorisnik: BaseSpecification<Porudzbina>
    {
         public PorudzbinaWithDostavaAndKorisnik()
        {
            AddInclude(x=>x.Dostava,y=>y.Korisnik,z=>z.StavkaPorudzbines);
        }
        public PorudzbinaWithDostavaAndKorisnik(int porudzbinaID) : base(x=>x.PorudzbinaId==porudzbinaID)
        {
            AddInclude(x=>x.Dostava,y=>y.Korisnik);    
        }
    }
}