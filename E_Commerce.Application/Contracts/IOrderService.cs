using E_Commerce.Application.Common;
using E_Commerce.Application.DTOs.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Contracts
{
    public interface IOrderService
    {
        Task<Result<OrderToReturnDTO>> CreateOrderAsync(OrderDTO orderDTO, string email, CancellationToken ct = default);
    }
}
