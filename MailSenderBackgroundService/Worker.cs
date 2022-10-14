using CreateEvent;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace MailSenderBackgroundService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _channel;

        private readonly IConfiguration _configuration;

        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _connectionFactory = new ConnectionFactory { HostName = _configuration.GetConnectionString("RabbitMQ") };
            _connection = _connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
            _logger.LogInformation($"Queue [{QueueNames.EmailQueue}] is waiting for messages.");

            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            System.Threading.Thread.Sleep(5000); //Exchange ve Queue Oluşturulması İçin Bekleme Süresi

            _channel.ExchangeDeclare(exchange: QueueNames.ExchangeName, type: ExchangeType.Topic, durable: true);
            _channel.QueueDeclare(queue: QueueNames.EmailQueue, durable: true, autoDelete: false, exclusive: false);
            _channel.QueueBind(queue: QueueNames.EmailQueue, exchange: QueueNames.ExchangeName, routingKey: QueueNames.ExchangeRoutingKey);

            // listen to the RabbitMQ messages, and send emails
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = JsonConvert.DeserializeObject<CreateOrderEvent>(Encoding.UTF8.GetString(body));

                _logger.LogInformation($"Siparis No: {message.OrderId}, Kullanici Adi: {message.CustomerName}, Email Adres: {message.CustomerEmail}");
            };

            _channel.BasicConsume(queue: QueueNames.EmailQueue,
                                 autoAck: true,
                                 consumer: consumer);


            await Task.CompletedTask;
        }
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);
            _connection.Close();
            _logger.LogInformation("RabbitMQ connection is closed.");
        }
    }
}
