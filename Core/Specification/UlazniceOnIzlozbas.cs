using Core.Entities;

namespace Core.Specification
{
    public class UlazniceOnIzlozbas : BaseSpecification<Ulaznica>
    {
        private List<Ulaznica> result;
        private UlazniceSpecParams parameters;

        public UlazniceOnIzlozbas()
        {
            AddInclude(x => x.Izlozba);
        }

        public UlazniceOnIzlozbas(UlazniceSpecParams? parameters) :
        base(u => (parameters.Grad == null || u.Izlozba.IzlozbaUMuzejus.Any(im => im.Muzej.Grad == parameters.Grad)) 
        && (string.IsNullOrEmpty(parameters.Search) || u.Izlozba.NazivIzlozbe.ToLower().Contains(parameters.Search)))
        {
            AddInclude(x => x.Izlozba);
            //AddOrderBy(x => x.Izlozba.NazivIzlozbe);
            ApplyPaging(parameters.PageSize*(parameters.PageIndex-1),parameters.PageSize);

            if (!string.IsNullOrEmpty(parameters.Sort))
            {
                switch (parameters.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(p => p.CenaUlaznice);
                        break;
                    case "priceDesc":
                        AddOrderByDescending(p => p.CenaUlaznice);
                        break;
                    case "izlozba":
                        AddOrderBy(p => p.Izlozba.NazivIzlozbe);
                        break;
                    default:
                        AddOrderBy(n => n.Izlozba.NazivIzlozbe);
                        break;

                }
            }
        }
        public UlazniceOnIzlozbas(int ulaznicaID) : base(x => x.UlaznicaId == ulaznicaID)
        {
            AddInclude(x => x.Izlozba);
        }

        public UlazniceOnIzlozbas(List<Ulaznica> result,string? grad, UlazniceSpecParams parameters):
        base(u=>u.Izlozba.IzlozbaUMuzejus.Any(im => im.Muzej.Grad == grad) && (string.IsNullOrEmpty(parameters.Search) || u.Izlozba.NazivIzlozbe.ToLower().Contains(parameters.Search)))
        {
            Ulaznicas= result;
            this.parameters = parameters;
           
            AddInclude(x => x.Izlozba);
            //AddOrderBy(x => x.Izlozba.NazivIzlozbe);
            ApplyPaging(parameters.PageSize*(parameters.PageIndex-1),parameters.PageSize);

            if (!string.IsNullOrEmpty(parameters.Sort))
            {
                switch (parameters.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(p => p.CenaUlaznice);
                        break;
                    case "priceDesc":
                        AddOrderByDescending(p => p.CenaUlaznice);
                        break;
                    case "izlozba":
                        AddOrderBy(p => p.Izlozba.NazivIzlozbe);
                        break;
                    default:
                        AddOrderBy(n => n.Izlozba.NazivIzlozbe);
                        break;

                }
            }
            
        }

        public List<Ulaznica> Ulaznicas { get; }
    }
}