using EShopFanerum.BotService;
using EShopFanerum.Service.BusService.Implementations;
using EShopFanerum.Service.BusService.Interfaces;
using MassTransit;
using Telegram.Bot;
using Telegram.Bot.Polling;

// Создаем хост приложения
var builder = Host.CreateApplicationBuilder(args);

// Конфигурация MassTransit с RabbitMQ
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("rmuser");
            h.Password("rmpassword");
        });
        
        // Дополнительные настройки если нужно
        cfg.ConfigureEndpoints(context);
    });
});

// Регистрируем наш сервис для отправки сообщений
builder.Services.AddScoped<ISenderService, MassTransitSenderService>();

// Регистрируем обработчик бота
builder.Services.AddSingleton<UpdateHandler>();

// Строим хост
var host = builder.Build();

// Получаем сервисы
var botClient = new TelegramBotClient("6682642475:AAEOS5HjeeKwxouAis6a2z1-Sxw2JdnJhx0");
var handler = host.Services.GetRequiredService<UpdateHandler>();
var busControl = host.Services.GetRequiredService<IBusControl>();

// Запускаем MassTransit
await busControl.StartAsync();

var cts = new CancellationTokenSource();
Console.CancelKeyPress += (_, _) => cts.Cancel();

// Запускаем бота
botClient.StartReceiving(
    handler,
    new ReceiverOptions(),
    cancellationToken: cts.Token);

Console.WriteLine("Bot started. Press ^C to stop");
await Task.Delay(-1, cancellationToken: cts.Token);
Console.WriteLine("Bot stopped");

// Останавливаем MassTransit при завершении
await busControl.StopAsync();