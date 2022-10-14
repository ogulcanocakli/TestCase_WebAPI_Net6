using CreateEvent;
using RabbitMQ.Client;

namespace RabbitMQService
{
    public class ConnectionRabbit
    {
        public static void CreateQueue()
        {
            ConnectionFactory _connectionFactory;
            IConnection _connection;
            IModel _channel;

            _connectionFactory = new ConnectionFactory { HostName = "localhost" };
            _connection = _connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(exchange: QueueNames.ExchangeName, type: ExchangeType.Topic, durable: true);
            _channel.QueueDeclare(queue: QueueNames.EmailQueue, durable: true, autoDelete: false, exclusive: false);
            _channel.QueueBind(queue: QueueNames.EmailQueue, exchange: QueueNames.ExchangeName, routingKey: QueueNames.ExchangeRoutingKey);

        }
    }
}