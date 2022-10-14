using Application.Dtos;
using MediatR;

namespace Application.Handlers.Products
{
    public class GetProductsQuery : IRequest<ApiResponse<List<ProductDto>>>
    {
        public string _Category { get; set; }
        public string GetProductsByCategoryKey => $"GetProductsByCategory/{_Category}";
        public string GetAllProductsKey => "GetAllProducts";

        public GetProductsQuery(string category)
        {
            _Category = category;
        }
    }
}
