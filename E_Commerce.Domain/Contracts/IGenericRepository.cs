using E_Commerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Contracts
{
    public interface IGenericRepository<TEntity,Tkey>where TEntity : BaseEntity<Tkey>,new()
    {
        Task<TEntity?> GetByIdAsync(Tkey id,CancellationToken ct);
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken ct);
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}
