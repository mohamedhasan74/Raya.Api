using Raya.Core.Entities;
using Raya.Core.Specification;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raya.Core.Interfaces
{
    public interface IGenericRepository <T> where T : BaseEntity
    {
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync(ISpecification<T> spec);
        Task AddAsync(T entity);
        void Delete(T entity);
        void Update(T entity);
    }
}
