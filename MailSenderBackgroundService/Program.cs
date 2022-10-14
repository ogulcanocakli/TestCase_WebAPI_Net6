using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using MailSenderBackgroundService;
using RabbitMQ.Client;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddHostedService<Worker>();

        services.AddSingleton(sp => new ConnectionFactory() { HostName = hostContext.Configuration.GetConnectionString("RabbitMQ") });

    })
    .Build();


await host.RunAsync();