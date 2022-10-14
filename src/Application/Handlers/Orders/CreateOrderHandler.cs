using Application.Interfaces;
using Application.Interfaces.Repositories;
using CreateEvent;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Serilog;
using System.Text;

namespace Application.Handlers.Orders
{
    public class CreateOrderHandler : IRequestHandler<CreateOrder, ApiResponse<int>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;

        private ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _channel;
        
        public CreateOrderHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork, ConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<int>> Handle(CreateOrder request, CancellationToken cancellationToken)
        {
            decimal totalAmount = 0;
            foreach (var item in request.ProductDetails)
            {
                totalAmount += item.UnitPrice * item.Amount;
            }

            var order = new Order
            {
                CustomerName = request.CustomerName,
                CustomerEmail = request.CustomerEmail,
                CustomerGSM = request.CustomerGSM,
                TotalAmount = totalAmount,
                OrderDetails = request.ProductDetails.Select(od => new OrderDetail
                {
                    ProductId = od.ProductId,
                    UnitPrice = od.UnitPrice,
                }).ToList()
            };
            await _orderRepository.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();

            //*********************************************************************** RabbitMQ

            _connection = _connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(exchange: QueueNames.ExchangeName, type: ExchangeType.Topic, durable: true);
            _channel.QueueDeclare(queue: QueueNames.EmailQueue, durable: true, autoDelete: false, exclusive: false);
            _channel.QueueBind(queue: QueueNames.EmailQueue, exchange: QueueNames.ExchangeName, routingKey: QueueNames.ExchangeRoutingKey);

            var orderCreatedEvent = new CreateOrderEvent { CustomerEmail = request.CustomerEmail, CustomerName = request.CustomerName, OrderId = order.Id };
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(orderCreatedEvent));

            _channel.BasicPublish(exchange: QueueNames.ExchangeName,
                                  routingKey: QueueNames.ExchangeRoutingKey,
                                  basicProperties: null,
                                  body: body);

            //***********************************************************************

            Log.Information($"Kullanıcı Adı: {order.CustomerName}, Sipariş No: {order.Id}");
            return new ApiResponse<int>(order.Id, Status.Success, "Sipariş oluşturuldu.");
        }
    }
}
