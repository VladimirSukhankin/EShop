using System.Diagnostics;
using System.Text.Json;
using EShopFanerum.Core.Helpers;
using EShopFanerum.Core.RabbitMQ;
using EShopFanerum.Core.RabbitMQ.Dto;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;


var botClient = new TelegramBotClient("6682642475:AAEOS5HjeeKwxouAis6a2z1-Sxw2JdnJhx0");

var cts = new CancellationTokenSource();
Console.CancelKeyPress += (_, _) => cts.Cancel();

var handler = new UpdateHandler(new RabbitMqSenderService());
var receiverOptions = new ReceiverOptions();
botClient.StartReceiving(handler, receiverOptions, cancellationToken: cts.Token);

Console.WriteLine("Bot started. Press ^C to stop");
await Task.Delay(-1,
    cancellationToken: cts
        .Token);
Console.WriteLine("Bot stopped");

public class UpdateHandler : IUpdateHandler
{
    private IRabbitMqSenderService _service;
    public UpdateHandler(IRabbitMqSenderService service)
    {
        _service = service;
    }
    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        Debug.WriteLine(JsonSerializer.Serialize(update));

        try
        {
            switch (update.Type)
            {
                case UpdateType.Message:
                {
                    var message = update.Message;
                    var user = message.From;

                    Console.WriteLine($"{user.FirstName} ({user.Id}) написал сообщение: {message.Text}");

                    var chat = message.Chat;

                    if (message.Type == MessageType.Text)
                    {
                        switch (message.Text)
                        {
                            case "/start":
                                var inlineKeyboard = new InlineKeyboardMarkup(
                                    new List<InlineKeyboardButton[]>()
                                    {
                                        new[]
                                        {
                                            InlineKeyboardButton.WithCallbackData("Сделать заказ", "setOrder"),
                                        },
                                    });

                                await botClient.SendTextMessageAsync(
                                    chat.Id,
                                    "Выбор действия",
                                    replyMarkup: inlineKeyboard);

                                break;
                            default:
                            {
                                await botClient.SendTextMessageAsync(
                                    chat.Id,
                                    "Сделайте выбор или введите /start");
                                break;
                            }
                        }
                    }

                    break;
                }
                case UpdateType.CallbackQuery:
                {
                    var callbackQuery = update.CallbackQuery;

                    var user = callbackQuery.From;

                    Console.WriteLine($"{user.FirstName} ({user.Id}) нажал на кнопку: {callbackQuery.Data}");

                    var chat = callbackQuery.Message.Chat;

                    switch (callbackQuery.Data)
                    {

                        case "setOrder":
                        {
                            var rnd = new Random();
                            var newOrder = new OrderDto()
                            {
                                Guid = Guid.NewGuid(),
                                Count = rnd.Next(),
                                Price = rnd.Next(),
                                GoodsIds = new List<long>() {11, 22, 33}
                            };
                            _service.SendMessage(newOrder);
                            
                            await botClient.SendTextMessageAsync(
                                chat.Id,
                                "Заказ отправлен /n. Для повторного заказа нажмите /start");
                            return;
                        }
                        default:
                        {
                            
                            return;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }
    
    public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception,
        CancellationToken cancellationToken)
    {
        Console.Error.WriteLine(exception);
        return Task.CompletedTask;
    }
}
