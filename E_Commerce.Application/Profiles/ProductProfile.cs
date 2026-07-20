using AutoMapper;
using E_Commerce.Application.DTOs.Products;
using E_Commerce.Domain.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Profiles
{
    internal class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductBrand, BrandDTO>();
            CreateMap<ProductType, TypeDTO>();
            CreateMap<Product, ProductDTO>()
                .ForMember(dest => dest.ProductBrand, opt => opt.MapFrom(p => p.ProductBrand.Name))
                .ForMember(dest => dest.ProductType, opt => opt.MapFrom(p => p.ProductType.Name))
                .ForMember(dest=>dest.PictureUrl,opt=>opt.MapFrom<PictureUrlResolver>());
        }
    }
}
