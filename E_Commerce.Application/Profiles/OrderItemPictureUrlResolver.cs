using AutoMapper;
using E_Commerce.Application.DTOs.Orders;
using E_Commerce.Domain.Models.Orders;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Profiles
{
    internal class OrderItemPictureUrlResolver : IValueResolver<OrderItem, OrderItemDTO, string>
    {
        private readonly UrlSetting _urlSettings;

        public OrderItemPictureUrlResolver(IOptions<UrlSetting> options)
        {
            this._urlSettings = options.Value;
        }
        public string Resolve(OrderItem source, OrderItemDTO destination, string destMember, ResolutionContext context)
        {
            var baseUrl = _urlSettings.BaseUrl.TrimEnd('/');
            var imagePath = source.Product.PictureUrl.TrimStart('/');
            return $"{baseUrl}/Files/{imagePath}";
        }
    }
}
