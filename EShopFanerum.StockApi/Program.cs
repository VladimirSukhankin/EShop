using EShopFanerum.Core.Helpers;
using EShopFanerum.Infrastructure.Extensions;
using EShopFanerum.Infrastructure.Mappings;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(StockProfile));

builder.Services.AddStockServices();

var settings = builder.Configuration.GetSection(nameof(RabbitMqSettings)).Get<RabbitMqSettings>();
if (settings != null)
{
    builder.Services.AddMassTransit(x =>
    {
        x.UsingRabbitMq((ctx, cfg) =>
        {
            cfg.Host(settings.HostNames, settings.VirtualHost, h =>
            {
                h.Username(settings.UserName);
                h.Password(settings.Password);
            });
                
            cfg.ConfigureEndpoints(ctx);
        });
    });
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();