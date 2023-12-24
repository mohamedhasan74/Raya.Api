using Raya.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raya.Core.Specification
{
    public class ProductsByConditionSpec : BaseSpecification<Product>
    {
        public ProductsByConditionSpec(ProductSpecParams queryParams) : 
            base(P => (string.IsNullOrEmpty(queryParams.Name) || (P.Name.ToLower().Contains(queryParams.Name.ToLower())))
              && (string.IsNullOrEmpty(queryParams.Description) || (P.Description.ToLower().Contains(queryParams.Description.ToLower())))
              && (!queryParams.Price.HasValue || (P.Price == queryParams.Price.Value))
              && (!queryParams.Quantity.HasValue || (P.Quantity == queryParams.Quantity.Value)))
        {
            
        }
    }
}
