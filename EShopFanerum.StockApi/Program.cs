using EShopFanerum.Infrastructure.Extensions;
using EShopFanerum.Infrastructure.Mappings;
using EShopFanerum.Infrastructure.Services;
using EShopFanerum.Infrastructure.Services.Impl;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(StockProfile));

builder.Services.AddScoped<IGoodService, GoodService>();
builder.Services.AddStockServices();
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