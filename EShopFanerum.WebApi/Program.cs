using System.Text;
using EShopFanerum.Core.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var appSettingsSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);
var appSettings = appSettingsSection.Get<AppSettings>();
var key = Encoding.ASCII.GetBytes(appSettings!.Secret);
builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/GetUsers", [Authorize]() =>
{
    return new[]
    {
        "John.Doe",
        "Jane.Doe",
        "Jewel.Doe",
        "Jayden.Doe",
    };
}).WithName("GetUsers");

app.MapGet("/RandomFail", () =>
{
    var randomValue = new Random().Next(0,2);
    //if(randomValue == 1)
    {
        throw new HttpRequestException("Random Failure");
    }

    //return "SomeData";
}).WithName("RandomFail");

app.MapGet("/RandomTimeout", async () =>
{
    var randomValue = new Random().Next(0, 2);
    //if (randomValue == 1)
    {
        await Task.Delay(10000);
    }

    //return "SomeData";
}).WithName("RandomTimeout");

app.UseAuthentication();
app.UseAuthorization();
app.Run();