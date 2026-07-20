using E_Commerce.Application.Common;
using E_Commerce.Application.DTOs.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Contracts
{
    public interface IProductService
    {
        Task<Result<PaginationResult<IReadOnlyList<ProductDTO>>>> GetAllProductsAsync(ProductQueryParams request, CancellationToken ct);
        Task<Result<IReadOnlyList<BrandDTO>>> GetAllBrandsAsync(CancellationToken ct);
        Task<Result<IReadOnlyList<TypeDTO>>> GetAllTypesAsync(CancellationToken ct);
        Task<Result<ProductDTO>> GetProductByIdAsync(int id, CancellationToken ct);



    }
}
