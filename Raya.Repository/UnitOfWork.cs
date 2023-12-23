using Raya.Core.Entities;
using Raya.Core.Interfaces;
using Raya.Repository.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raya.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private Hashtable _Repositories;
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            _Repositories = new Hashtable();
        }
        public IGenericRepository<T> Repository<T>() where T : BaseEntity
        {
            var type = typeof(T).Name;
            if (!_Repositories.ContainsKey(type))
            {
                var repo = new GenericRepository<T>(_context);
                _Repositories.Add(type, repo);
            }
            return _Repositories[type] as IGenericRepository<T>;
        }
        public async Task<int> CompleteAsyn()
        {
            return await _context.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }
    }
}
