using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Specification
{
    public class KorisnikWithTip : BaseSpecification<Korisnik>

    {
        public KorisnikWithTip()
        {
            AddInclude(x => x.TipKorisnika, y => y.Porudzbinas);
            //AddInclude(x => x.TipKorisnika,z=>z.Porudzbinas,y => y.Porudzbinas.Select(p => p.StavkaPorudzbines));
            
        }
        public KorisnikWithTip(int korisnikID) : base(x => x.KorisnikId == korisnikID)
        {
            AddInclude(x => x.TipKorisnika, y => y.Porudzbinas);


        }
    }
}