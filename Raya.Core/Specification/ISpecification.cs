using Raya.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Raya.Core.Specification
{
    public interface ISpecification <T> where T : BaseEntity
    {
        Expression<Func<T, bool>> Criteria { get; set; }
    }
}
