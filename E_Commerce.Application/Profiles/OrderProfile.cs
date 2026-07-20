using AutoMapper;
using E_Commerce.Application.DTOs.Identity;
using E_Commerce.Application.DTOs.Orders;
using E_Commerce.Domain.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Profiles
{
    internal class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<AddressDTO, OrderAddress>().ReverseMap();

            CreateMap<Order, OrderToReturnDTO>()
                .ForMember(x => x.DeliveryMethod, dest => dest.MapFrom(o => o.DeliveryMethod.ShortName))
                .ForMember(x => x.DeliveryCost, dest => dest.MapFrom(o => o.DeliveryMethod.Cost)).ReverseMap();
            CreateMap<OrderItem, OrderItemDTO>()
                .ForMember(x => x.ProductId, dest => dest.MapFrom(o => o.Product.ProductId))
                .ForMember(x => x.ProductName, dest => dest.MapFrom(o => o.Product.ProductName))
                .ForMember(x => x.PictureUrl, dest => dest.MapFrom<OrderItemPictureUrlResolver>());
        }
    }
}
