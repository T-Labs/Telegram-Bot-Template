using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using TelegramBotTemplate.Services.Interfaces;

namespace TelegramBotTemplate.Services
{
    public class BotEventsHandler : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public BotEventsHandler(
            IServiceScopeFactory scopeFactory, ITelegramBotClient botClient)
        {
            _scopeFactory = scopeFactory;
            botClient.OnMessage += Bot_OnMessage;
            botClient.OnCallbackQuery += Bot_OnCallbackQuery;
            botClient.StartReceiving();
        }

        private async void Bot_OnMessage(object sender, MessageEventArgs messageEventArgs)
        {
            Console.WriteLine("Event OnMessage");
            var message = messageEventArgs.Message;

            using (var scope = _scopeFactory.CreateScope())
            {
                var scenariosService = scope.ServiceProvider.GetRequiredService<IScenariosService>();
                if (message.Text != null)
                {
                    if (message.Text == "/start" || message.Text == "/")
                    {
                        await scenariosService.StartCommand(message);
                        return;
                    }

                    await scenariosService.TextCommand(message);
                }
            }
        }

        private async void Bot_OnCallbackQuery(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
        {
            CallbackQuery query = callbackQueryEventArgs.CallbackQuery;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }
    }
}
