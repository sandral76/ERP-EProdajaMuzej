using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specification
{
    public class IzlozbeUMuzeju : BaseSpecification<IzlozbaUMuzeju>
    {
        public IzlozbeUMuzeju()
        {
            AddInclude(x=>x.Muzej,y=>y.Izlozba);

        }
        public IzlozbeUMuzeju(int muzejID,int izlozbaID) : base(x=>x.MuzejId==muzejID && x.IzlozbaId==izlozbaID)
        {
            AddInclude(x=>x.Muzej,y=>y.Izlozba);
        }
      
    }
}