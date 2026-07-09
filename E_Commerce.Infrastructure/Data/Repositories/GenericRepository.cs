using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Models;
using E_Commerce.Infrastructure.Data.DBContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Data.Repositories
{
    internal class GenericRepository<TEntity, TKey>(StoreDBContext dbContext)
     : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>, new()
    {
        public async Task<TEntity?> GetByIdAsync(TKey id, CancellationToken ct)
            => await dbContext.Set<TEntity>().FindAsync(id, ct);

        public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken ct)
            => await dbContext.Set<TEntity>().ToListAsync(ct);

        public void Add(TEntity entity) => dbContext.Set<TEntity>().Add(entity);
        public void Update(TEntity entity) => dbContext.Set<TEntity>().Update(entity);
        public void Delete(TEntity entity) => dbContext.Set<TEntity>().Remove(entity);
    }
}
