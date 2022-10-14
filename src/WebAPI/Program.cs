using Application;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Context;
using RabbitMQ.Client;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//*********************************************************************

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddSingleton(sp => new ConnectionFactory() { HostName = builder.Configuration.GetConnectionString("RabbitMQ") });

var logConnectionString = builder.Configuration.GetConnectionString("MySQLConnection");

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.MySQL(connectionString: logConnectionString, tableName: "Log")
    .CreateLogger();

//*********************************************************************

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<CaseDbContext>();
    if (dataContext.Database.GetPendingMigrations().Any())
    {
        dataContext.Database.Migrate();
    }
}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
