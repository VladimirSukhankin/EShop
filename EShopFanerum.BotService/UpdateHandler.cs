using System.Diagnostics;
using System.Text.Json;
using EShopFanerum.Service.BusService.Interfaces;
using EShopFanerum.Service.BusService.Model;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace EShopFanerum.BotService;

public class UpdateHandler(ISenderService service) : IUpdateHandler
{
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
                    var user = message?.From;

                    Console.WriteLine($"{user?.FirstName} ({user?.Id}) написал сообщение: {message?.Text}");

                    var chat = message?.Chat;
                    if (chat == null)
                    {
                        return;
                    }
                    if (message?.Type == MessageType.Text)
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

                                await botClient.SendTextMessageAsync(chat.Id, "Выбор действия", replyMarkup: inlineKeyboard, cancellationToken: cancellationToken);

                                break;
                            default:
                            {
                                await botClient.SendTextMessageAsync(chat.Id, "Сделайте выбор или введите /start", cancellationToken: cancellationToken);
                                break;
                            }
                        }
                    }

                    break;
                }
                case UpdateType.CallbackQuery:
                {
                    var callbackQuery = update.CallbackQuery;
                    if (callbackQuery == null)
                    {
                        return;
                    }
                    
                    var user = callbackQuery.From;

                    Console.WriteLine($"{user.FirstName} ({user.Id}) нажал на кнопку: {callbackQuery.Data}");

                    var chat = callbackQuery.Message?.Chat;
                    if (chat == null)
                    {
                        return;
                    }
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
                                GoodsIds = [11, 22, 33]
                            };
                            await service.SendMessage(newOrder);
                            
                            await botClient.SendTextMessageAsync(chat.Id, "Заказ отправлен /n. Для повторного заказа нажмите /start", cancellationToken: cancellationToken);
                            return;
                        }
                        default:
                        {
                            
                            return;
                        }
                    }
                }
                default:
                    throw new ArgumentOutOfRangeException();
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