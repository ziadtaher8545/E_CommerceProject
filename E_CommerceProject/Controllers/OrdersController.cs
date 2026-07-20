using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceProject.Controllers;

public class OrdersController : BaseController
{
    private readonly IOrderService orderService;

    public OrdersController(IOrderService orderService)
    {
        this.orderService = orderService;
    }
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<OrderToReturnDTO>> CreateOrder([FromBody] OrderDTO order)
    {
        return ToActionResult(await orderService.CreateOrderAsync(order, GetEmailByToken()));
    }
}
