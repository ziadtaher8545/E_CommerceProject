using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Baskets;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceProject.Controllers
{

    public class BasketsController : BaseController
    {
        private readonly IBasketService _basketService;

        public BasketsController(IBasketService basketService)
        {
            this._basketService = basketService;
        }
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BasketDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<BasketDTO>> GetBasket(string id, CancellationToken ct)
        {
            var basket = await _basketService.GetBasketAsync(id, ct);
            return ToActionResult(basket);

        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteBasket(string id, CancellationToken ct)
        {
            var result = await _basketService.DeleteBasketAsync(id, ct);
            return ToActionResult(result);
        }
        [HttpPost]
        public async Task<ActionResult<BasketDTO>> CreateOrUpdateBasket(BasketDTO basket, CancellationToken ct)
        {
            var result = await _basketService.CreateOrUpdateBasketAsync(basket, ct: ct);
            return ToActionResult<BasketDTO>(result);
        }
    }
}
