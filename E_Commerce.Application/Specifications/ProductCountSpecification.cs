using E_Commerce.Application.Common;
using E_Commerce.Domain.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Specifications
{
    internal class ProductCountSpecification : BaseSpecification<Product, int>
    {
        public ProductCountSpecification(ProductQueryParams req) :
            base(p => (!req.BrandId.HasValue || p.BrandId == req.BrandId) &&
            (!req.TypeId.HasValue || p.TypeId == req.TypeId) &&
            (string.IsNullOrWhiteSpace(req.SearchText) || p.Name.Contains(req.SearchText)))
        {

        }
    }
}
