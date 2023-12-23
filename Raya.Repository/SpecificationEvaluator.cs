using Raya.Core.Entities;
using Raya.Core.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raya.Repository
{
    public class SpecificationEvaluator <T> where T : BaseEntity
    {
        public static IQueryable<T> GetQuery(IQueryable<T> InputQuery, ISpecification<T> spec)
        {
            var query = InputQuery;
            if(spec.Criteria is not null)
                query = query.Where(spec.Criteria);
            return query;
        }
    }
}
