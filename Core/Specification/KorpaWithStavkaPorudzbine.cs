using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Specification
{
    public class KorpaWithStavkaPorudzbine : BaseSpecification<Korpa>
    {
        public KorpaWithStavkaPorudzbine()
        {
            AddInclude(k => k.StavkaPorudzbines);
            
        }
        public KorpaWithStavkaPorudzbine(int korpaID) : base(x => x.KorpaId == korpaID)
        {
            AddInclude(k => k.StavkaPorudzbines);
            
        }
        
    }
}