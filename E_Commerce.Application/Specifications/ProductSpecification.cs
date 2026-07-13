using E_Commerce.Application.Common;
using E_Commerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Specifications
{
    internal class ProductSpecification : BaseSpecification<Product, int>
    {

        public ProductSpecification(Expression<Func<Product, bool>> critria = null) : base(critria)
        {
            AddInclude(p => p.ProductType);
            AddInclude(p => p.ProductBrand);
        }
        public ProductSpecification(ProductQueryParams req) :
            base(p => (!req.BrandId.HasValue || p.BrandId == req.BrandId) &&
            (!req.TypeId.HasValue || p.TypeId == req.TypeId) &&
            (string.IsNullOrWhiteSpace(req.SearchText) || p.Name.Contains(req.SearchText)))
        {


            AddInclude(p => p.ProductType);
            AddInclude(p => p.ProductBrand);
            switch (req.SortingOptions)
            {

                case ProductSortingOptions.Name:
                    AddOrderBy(p => p.Name);
                    break;
                case ProductSortingOptions.NameDesc:
                    AddOrderByDesc(p => p.Name);
                    break;
                case ProductSortingOptions.Price:
                    AddOrderBy(p => p.Price);
                    break;
                case ProductSortingOptions.PriceDesc:
                    AddOrderByDesc(p => p.Price);
                    break;
                default:
                    AddOrderBy(p => p.Id);
                    break;
            }
            AddPagination(req.PageSize, req.PageNumber);
        }

        public ProductSpecification(int id) : base(p => p.Id == id)
        {

            AddInclude(p => p.ProductType);
            AddInclude(p => p.ProductBrand);
        }
    }
}
