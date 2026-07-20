using E_Commerce.Application.Common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Products;
using E_CommerceProject.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceProject.Controllers
{

    public class ProductsController : BaseController
    {

        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }


        [HttpGet]
        [RedisCache]
        public async Task<ActionResult<PaginationResult<IReadOnlyList<ProductDTO>>>> GetProducts([FromQuery] ProductQueryParams request, CancellationToken ct)
        {
            var result = await _productService.GetAllProductsAsync(request, ct);
            return ToActionResult(result);

        }
        [HttpGet("Brands")]
        public async Task<ActionResult<IReadOnlyList<BrandDTO>>> GetBrands(CancellationToken ct)
       => ToActionResult(await _productService.GetAllBrandsAsync(ct));
        [HttpGet("Types")]
        [ProducesResponseType(typeof(ProductDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]

        public async Task<ActionResult<IReadOnlyList<TypeDTO>>> GetTypes(CancellationToken ct)
     => ToActionResult(await _productService.GetAllTypesAsync(ct));

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProductById([FromRoute] int id, CancellationToken ct)
     => ToActionResult(await _productService.GetProductByIdAsync(id, ct));





    }
}
