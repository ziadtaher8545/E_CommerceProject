using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Models.Products;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Data.Specifications
{
    internal static class SpecificationEvaluator
    {
        internal static IQueryable<TEntity> CreateQuery<TEntity, Tkey>(IQueryable<TEntity> baseQuery, ISpecification<TEntity, Tkey> spec) where TEntity : BaseEntity<Tkey>, new()
        {
            var curQuery = baseQuery;

            if (spec.Criteria != null)
            {
                curQuery = curQuery.Where(spec.Criteria);
            }
            if (spec.IncludeExpresions.Any())
            {
                curQuery = spec.IncludeExpresions.Aggregate(curQuery, (cur, nextExp) => cur.Include(nextExp));

            }
            if (spec.OrderBy != null)
                curQuery = curQuery.OrderBy(spec.OrderBy);
            else if (spec.OrderByDesc != null)
                curQuery = curQuery.OrderByDescending(spec.OrderByDesc);

            if (spec.IsPagination)
                curQuery = curQuery.Skip(spec.Skip).Take(spec.Take);
            return curQuery;

        }
    }
}
