using AutoMapper;
using E_Commerce.Application.Common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Orders;
using E_Commerce.Application.Specifications;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Models.Orders;
using E_Commerce.Domain.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Services
{
    internal class OrderService : IOrderService
    {
        private readonly IBasketRepository basketRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public OrderService(IBasketRepository basketRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.basketRepository = basketRepository;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<Result<OrderToReturnDTO>> CreateOrderAsync(OrderDTO orderDTO, string email, CancellationToken ct = default)
        {

            var basket = await basketRepository.GetBasketAsync(orderDTO.BasketId, ct);
            if (basket == null)
                return Error.NotFound("Basket Not Found", $"Basket With Id {orderDTO.BasketId} Is not Found");
            if (basket.Items.Count == 0)
                return Error.Validation("Basket is Empty", $"Can Not Create Order With Basket With Id {orderDTO.BasketId}");
            var orderItems = new List<OrderItem>();
            var productIds = basket.Items.Select(b => b.Id).ToHashSet();
            var spec = new ProductWithIdSpecifaction(productIds);
            var products = (await unitOfWork.GetRepository<Product, int>().GetAllAsync(spec, ct)).ToDictionary(p => p.Id);
            foreach (var item in basket.Items)
            {
                if (!products.TryGetValue(item.Id, out var product))
                    return Result<OrderToReturnDTO>.Fail(Error.NotFound("Product Not Found", $"Product With Id {item.Id} Is Not Found "));

                orderItems.Add(new OrderItem
                {
                    Price = product.Price,
                    Quantity = item.Quantity,
                    Product = new ProductItemOrdered
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        PictureUrl = product.PictureUrl
                    }
                });
            }
            var shipAddress = mapper.Map<OrderAddress>(orderDTO.ShippingAddress);
            var deliveryMethod = await unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(orderDTO.DeliveryMethodId, ct);
            if (deliveryMethod == null) return Error.NotFound("DeliveryMethod is Not Found", $"DeliveryMethod With {orderDTO.DeliveryMethodId} is Not Found");
            var subtotal = orderItems.Sum(o => o.Quantity * o.Price);


            var order = new Order(email, orderItems, shipAddress, deliveryMethod, subtotal);
            unitOfWork.GetRepository<Order, Guid>().Add(order);
            var result = await unitOfWork.SaveChangesAsync(ct);
            if (result == 0)
                return Error.Failure("Failure To create Order", $"Failed To Create Order");
            await basketRepository.DeleteBasketAsync(basket.Id, ct);

            return mapper.Map<OrderToReturnDTO>(order);



        }
    }
}
