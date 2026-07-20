using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Models.Products;
using E_Commerce.Infrastructure.Data.DBContexts;
using E_Commerce.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Data
{
    internal class UnitOfWork(StoreDBContext dbContext) : IUnitOfWork
    {
        private readonly Dictionary<string, object> repositories = [];

        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>()
            where TEntity : BaseEntity<TKey>, new()
        {
            var typeName = typeof(TEntity).Name;
            if (repositories.TryGetValue(typeName, out var value))
                return (IGenericRepository<TEntity, TKey>)value;

            var Repo = new GenericRepository<TEntity, TKey>(dbContext);
            repositories[typeName] = Repo;
            return Repo;
        }

        public Task<int> SaveChangesAsync(CancellationToken ct = default)
            => dbContext.SaveChangesAsync(ct);
    }
}
