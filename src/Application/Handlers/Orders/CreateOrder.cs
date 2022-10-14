using Application.Dtos;
using MediatR;

namespace Application.Handlers.Orders
{
    public class CreateOrder : IRequest<ApiResponse<int>>
    {
        public string CustomerName { get; set; }
        public string CustomerGSM { get; set; }
        public string CustomerEmail { get; set; }
        public List<ProductDetail> ProductDetails { get; set; }
    }
}
