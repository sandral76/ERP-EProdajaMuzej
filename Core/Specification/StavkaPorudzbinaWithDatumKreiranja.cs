using Core.Entities;

namespace Core.Specification
{
    public class StavkaPorudzbinaWithDatumKreiranja: BaseSpecification<StavkaPorudzbine>
    {
       

        public StavkaPorudzbinaWithDatumKreiranja()
        {
            AddInclude(x=>x.Porudzbina);
        }
        public StavkaPorudzbinaWithDatumKreiranja(int stavkaPorudzbineID,int ulaznicaID) : base(x=>x.StavkaPorudzbineId==stavkaPorudzbineID && x.UlaznicaId==ulaznicaID)
        {
            AddInclude(x=>x.Porudzbina);       
        }
         
    }
}