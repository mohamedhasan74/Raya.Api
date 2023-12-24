using Raya.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raya.Core.Specification
{
    public class ProductByNameSpec : BaseSpecification<Product>
    {
        public ProductByNameSpec(string name) : base (P => P.Name.ToLower() == name.ToLower())
        {
            
        }
    }
}
