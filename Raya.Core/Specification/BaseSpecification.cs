using Raya.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Raya.Core.Specification
{
    public class BaseSpecification<T> : ISpecification<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>> Criteria { get; set; }
        public BaseSpecification()
        {
            
        }
        public BaseSpecification(Expression<Func<T, bool>> _Criteria)
        {
            Criteria = _Criteria; 
        }
    }
}
