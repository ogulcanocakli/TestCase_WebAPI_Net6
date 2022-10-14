using Application.Dtos;
using Application.Handlers.Products;
using Application;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpGet]
        public async Task<ApiResponse<List<ProductDto>>> GetProducts([FromQuery] string? category = "")
        {
            return await _mediator.Send(new GetProductsQuery(category.ToLower()));
        }
    }
}
