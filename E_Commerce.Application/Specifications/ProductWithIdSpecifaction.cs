using E_Commerce.Domain.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Specifications
{
    internal class ProductWithIdSpecifaction : BaseSpecification<Product, int>
    {
        public ProductWithIdSpecifaction(HashSet<int> products) : base(p => products.Contains(p.Id))
        {
        }
    }
}
