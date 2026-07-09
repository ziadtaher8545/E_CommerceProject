using AutoMapper;
using E_Commerce.Application.Common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Product;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<Result<IReadOnlyList<ProductDTO>>> GetAllProductsAsync(CancellationToken ct)
        {

            var data = await _unitOfWork.GetRepository<Product, int>().GetAllAsync(ct);
            var mappedData = _mapper.Map<IReadOnlyList<ProductDTO>>(data);
            return Result<IReadOnlyList<ProductDTO>>.Ok(mappedData);
        }



        public async Task<Result<IReadOnlyList<TypeDTO>>> GetAllTypesAsync(CancellationToken ct)
        {

            var data = await _unitOfWork.GetRepository<ProductType, int>().GetAllAsync(ct);
            var mappedData = _mapper.Map<IReadOnlyList<TypeDTO>>(data);
            return Result<IReadOnlyList<TypeDTO>>.Ok(mappedData);
        }


        public async Task<Result<ProductDTO>> GetProductByIdAsync(int id, CancellationToken ct)
        {

            var data = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(id, ct);
            if (data == null)
                return new Error("Product.NotFound", $"Product With {id} Wasn't Found ", ErrorType.NotFound);

            var mappedData = _mapper.Map<ProductDTO>(data);
            return mappedData;
        }


    }
}
