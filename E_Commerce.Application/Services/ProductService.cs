using AutoMapper;
using E_Commerce.Application.Common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Product;
using E_Commerce.Application.Specifications;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Models.Products;

namespace E_Commerce.Application.Services
{
    internal class ProductService(IUnitOfWork _unitOfWork, IMapper _mapper) : IProductService
    {
        public async Task<Result<IReadOnlyList<BrandDTO>>> GetAllBrandsAsync(CancellationToken ct)
        {
            var data = await _unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync(ct);
            var mappedData = _mapper.Map<IReadOnlyList<BrandDTO>>(data);
            return Result<IReadOnlyList<BrandDTO>>.Ok(mappedData);

        }

        public async Task<Result<PaginationResult<IReadOnlyList<ProductDTO>>>> GetAllProductsAsync(ProductQueryParams request, CancellationToken ct)
        {
            var spec = new ProductSpecification(request);
            var data = await _unitOfWork.GetRepository<Product, int>().GetAllAsync(spec, ct);
            var mappedData = _mapper.Map<IReadOnlyList<ProductDTO>>(data);
            var countSpec = new ProductCountSpecification(request);
            var count = await _unitOfWork.GetRepository<Product, int>().GetCountAsync(countSpec, ct);
            var result = new PaginationResult<IReadOnlyList<ProductDTO>>(request.PageNumber, request.PageSize, count, mappedData);
            return Result<PaginationResult<IReadOnlyList<ProductDTO>>>.Ok(result);

        }



        public async Task<Result<IReadOnlyList<TypeDTO>>> GetAllTypesAsync(CancellationToken ct)
        {
            var spec = new ProductSpecification();

            var data = await _unitOfWork.GetRepository<ProductType, int>().GetAllAsync(ct);
            var mappedData = _mapper.Map<IReadOnlyList<TypeDTO>>(data);
            return Result<IReadOnlyList<TypeDTO>>.Ok(mappedData);
        }


        public async Task<Result<ProductDTO>> GetProductByIdAsync(int id, CancellationToken ct)
        {

            var spec = new ProductSpecification(id);
            var data = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(spec, id, ct);
            if (data == null)
                return new Error("Product.NotFound", $"Product With {id} Wasn't Found ", ErrorType.NotFound);

            var mappedData = _mapper.Map<ProductDTO>(data);
            return mappedData;
        }


    }
}
