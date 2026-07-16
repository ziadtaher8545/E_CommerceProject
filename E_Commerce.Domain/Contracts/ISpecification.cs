using E_Commerce.Domain.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Contracts
{
    public interface ISpecification<TEntity, Tkey> where TEntity : BaseEntity<Tkey>, new()
    {
        ICollection<Expression<Func<TEntity, object>>> IncludeExpresions { get; }
        Expression<Func<TEntity, bool>> Criteria { get; }

        Expression<Func<TEntity, object>> OrderBy { get; }
        Expression<Func<TEntity, object>> OrderByDesc { get; }
        int Take { get; }
        int Skip { get; }
        bool IsPagination { get; }

    }
}
