using AutoMapper;
using E_Commerce.Application.DTOs.Baskets;
using E_Commerce.Domain.Models.Baskets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Profiles
{
    internal class BasketProfile : Profile
    {
        public BasketProfile()
        {
            CreateMap<BasketDTO, CustomerBasket>().ReverseMap();
            CreateMap<BasketItemDTO, BasketItem>().ReverseMap();

        }
    }
}
