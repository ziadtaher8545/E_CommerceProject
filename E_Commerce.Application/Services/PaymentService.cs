using AutoMapper;
using E_Commerce.Application.Common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Baskets;
using E_Commerce.Application.Specifications;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Models.Orders;
using E_Commerce.Domain.Models.Products;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Services;

internal class PaymentService : IPaymentService
{
    private readonly IBasketRepository _basketRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPaymentGatway _paymentGateway;
    private readonly PaymentGatwaySettings _stripe;
    private readonly IMapper _mapper;

    public PaymentService(IBasketRepository basketRepository, IUnitOfWork unitOfWork, IPaymentGatway paymentGateway, IOptions<PaymentGatwaySettings> stripeSettings, IMapper mapper)
    {
        _basketRepository = basketRepository;
        _unitOfWork = unitOfWork;
        _paymentGateway = paymentGateway;
        _stripe = stripeSettings.Value;
        _mapper = mapper;
    }

    public async Task<Result<BasketDTO>> CreateOrUpdatePaymentIntentAsync(string basketId, CancellationToken ct = default)
    {
        var basket = await _basketRepository.GetBasketAsync(basketId, ct);
        if (basket == null)
            return Result<BasketDTO>.Fail(Error.NotFound("Basket Not Found", $"The Basket With Id {basketId} is not found"));
        if (basket.Items.Count == 0)
            return Result<BasketDTO>.Fail(Error.Validation("Basket is Empty", $"Can Not Create Order With Basket With Id {basketId}"));


        var productRepo = _unitOfWork.GetRepository<Product, int>();
        var productsIds = basket.Items.Select(i => i.Id).ToHashSet();
        var products = (await productRepo.GetAllAsync(new ProductWithIdSpecifaction(productsIds), ct)).ToDictionary(x => x.Id);

        foreach (var item in basket.Items)
        {
            if (!products.TryGetValue(item.Id, out var product))
                return Result<BasketDTO>.Fail(Error.NotFound("Product Not Found", $"Product With Id {item.Id} Is Not Found "));

            item.Price = product.Price;
        }

        var deliveryRepo = _unitOfWork.GetRepository<DeliveryMethod, int>();
        var deliveryMethod = await deliveryRepo.GetByIdAsync(basket.DeliveryMethodId, ct);
        if (deliveryMethod == null)
            return Result<BasketDTO>.Fail(Error.NotFound("Delivery Method Not Found", $"DeliveryMethod With Id {basket.DeliveryMethodId} Is Not Found "));

        basket.ShippingPrice = deliveryMethod.Cost;
        var subtotal = basket.Items.Sum(p => p.Quantity * p.Price);
        var amount = (long)Math.Round((subtotal + deliveryMethod.Cost) * 100m);
        if (!string.IsNullOrEmpty(basket.PaymentIntendId))
        {
            var payment = await _paymentGateway.UpdatePaymentIntentAsync(basket.PaymentIntendId, amount, ct);
            basket.PaymentIntendId = payment.PaymentIntendId;
            basket.ClientSecert = payment.ClientSecert;


        }
        else
        {
            var payment = await _paymentGateway.CreatePaymentIntentAsync(amount, _stripe.DefaultCurrency, ct);
            basket.PaymentIntendId = payment.PaymentIntendId;
            basket.ClientSecert = payment.ClientSecert;


        }
        await _basketRepository.CreateOrUpdateBasketAsync(basket, ct: ct);
        return _mapper.Map<BasketDTO>(basket);




    }
}
