using AutoMapper;
using E_Commerce.Application.DTOs.Products;
using E_Commerce.Domain.Models.Products;
using Microsoft.Extensions.Options;

namespace E_Commerce.Application.Profiles
{
    public class PictureUrlResolver : IValueResolver<Product, ProductDTO, string>
    {
        private readonly IOptions<UrlSetting> _urlSettings;

        public PictureUrlResolver(IOptions<UrlSetting> options)
        {
            _urlSettings = options;
        }


        public string Resolve(Product source, ProductDTO destination, string destMember, ResolutionContext context)
        {
            var baseUrl = _urlSettings.Value.BaseUrl.TrimEnd('/');
            var imagePath = source.PictureUrl.TrimStart('/');
            return $"{baseUrl}/Files/{imagePath}";
        }
    }
    public class UrlSetting
    {
        public string BaseUrl { get; set; }
    }
}