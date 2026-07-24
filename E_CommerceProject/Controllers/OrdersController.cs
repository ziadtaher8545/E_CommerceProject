using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceProject.Controllers;

[Authorize]
public class OrdersController : BaseController
{
    private readonly IOrderService orderService;

    public OrdersController(IOrderService orderService)
    {
        this.orderService = orderService;
    }
    [HttpPost]
    public async Task<ActionResult<OrderToReturnDTO>> CreateOrder([FromBody] OrderDTO order)
    {
        return ToActionResult(await orderService.CreateOrderAsync(order, GetEmailByToken()));
    }
    [AllowAnonymous]
    [HttpGet("DeliveryMethods")]
    public async Task<ActionResult<IReadOnlyList<DeliveryMethodDTO>>> GetDeliveryMethods(CancellationToken ct) => ToActionResult(await orderService.GetDeliveryMethodAsync(ct));
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<OrderToReturnDTO>>> GetUserOrders(CancellationToken ct)
        => ToActionResult(await orderService.GetUserOrdersAsync(GetEmailByToken(), ct));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<OrderToReturnDTO>> GetOrderById([FromQuery]Guid id, CancellationToken ct)
    => ToActionResult(await orderService.GetOrderByIdAndEmailAsync(id, GetEmailByToken(), ct));

}
