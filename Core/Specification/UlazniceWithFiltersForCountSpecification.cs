using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Specification
{
    public class UlazniceWithFiltersForCountSpecification : BaseSpecification<Ulaznica>
    {
        private UlazniceSpecParams parameters;

        public UlazniceWithFiltersForCountSpecification(UlazniceSpecParams parameters) :
        base(x=>string.IsNullOrEmpty(parameters.Search) || x.Izlozba.NazivIzlozbe.ToLower().Contains(parameters.Search))
        {

        }

    }
}