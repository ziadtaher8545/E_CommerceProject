using AutoMapper;
using E_Commerce.Application.Common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Baskets;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Models.Baskets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Services
{
    internal class BasketService : IBasketService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;



        public BasketService(IBasketRepository basketRepository, IMapper mapper)
        {
            this._basketRepository = basketRepository;
            _mapper = mapper;
        }
        public async Task<Result<BasketDTO>> CreateOrUpdateBasketAsync(BasketDTO basket, TimeSpan? timeToLive = null, CancellationToken ct = default)
        {
            var mappedData = _mapper.Map<CustomerBasket>(basket);


            var result = await _basketRepository.CreateOrUpdateBasketAsync(mappedData);
            if (basket == null)
                return Result<BasketDTO>.Fail(Error.Failure("BasketDelete.Failure", "Can Not Delete Basket"));
            return Result<BasketDTO>.Ok(basket);
        }

        public async Task<Result<bool>> DeleteBasketAsync(string basketId, CancellationToken ct = default)
        {
            var result = await _basketRepository.DeleteBasketAsync(basketId);
            if (result)
                return Result<bool>.Ok(true);
            return Result<bool>.Fail(Error.Failure("BasketDelete.Failure", "Can Not Delete Basket"));
        }

        public async Task<Result<BasketDTO>> GetBasketAsync(string basketId, CancellationToken ct = default)
        {
            var result = await _basketRepository.GetBasketAsync(basketId);
            if (result == null)
                return Result<BasketDTO>.Fail(Error.NotFound("Basket Not Found"));
            return Result<BasketDTO>.Ok(_mapper.Map<BasketDTO>(result));
        }
    }
}
