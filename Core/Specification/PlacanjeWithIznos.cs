using Core.Entities;

namespace Core.Specification
{
    public class PlacanjeWithIznos : BaseSpecification<Placanje>
    {
        public PlacanjeWithIznos()
        {
            AddInclude(x=>x.Porudzbina);
        }
        public PlacanjeWithIznos(int placanjeID) : base(x=>x.PlacanjeId==placanjeID)
        {
            AddInclude(x=>x.Porudzbina);
        }
    }
}