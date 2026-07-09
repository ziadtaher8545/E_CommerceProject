using E_Commerce.Application.Common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }


        [HttpGet]
        public async Task<ActionResult<Result<IReadOnlyList<ProductDTO>>>> GetProducts(CancellationToken ct)
        {
            var result = await _productService.GetAllProductsAsync(ct);
            return Ok(result);

        }
        [HttpGet("Brands")]
        public async Task<ActionResult<Result<IReadOnlyList<BrandDTO>>>> GetBrands(CancellationToken ct)
       => Ok(await _productService.GetAllBrandsAsync(ct));
        [HttpGet("Types")]
        public async Task<ActionResult<Result<IReadOnlyList<TypeDTO>>>> GetTypes(CancellationToken ct)
     => Ok(await _productService.GetAllBrandsAsync(ct));

        [HttpGet("{id}")]
        public async Task<ActionResult<Result<ProductDTO>>> GetProductById([FromRoute] int id, CancellationToken ct)
     => Ok(await _productService.GetProductByIdAsync(id, ct));





    }
}
