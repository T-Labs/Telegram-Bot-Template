using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBotTemplate.Services.Interfaces;

namespace TelegramBotTemplate.Services
{
    public class MessageSender : IMessageSender
    {
        private readonly ITelegramBotClient _botClient;

        public MessageSender(
            ITelegramBotClient botClient
            )
        {
            _botClient = botClient;
        }

        public Task SendErrorUnknownCommand(Chat chat)
        {
            return SendOrUpdate(chat, $"Unknown Command");
        }

        public async Task<Message> SendOrUpdate(ChatId chat, string text, IReplyMarkup replyMarkup = null, int? messageId = null)
        {
            try
            {
                Message message;
                if (messageId.HasValue)
                {
                    message = await _botClient.EditMessageTextAsync(
                        messageId: messageId.Value,
                        chatId: chat,
                        text: text,
                        replyMarkup: (InlineKeyboardMarkup)replyMarkup,
                        parseMode: ParseMode.Html
                    );
                }
                else
                {
                    message = await _botClient.SendTextMessageAsync(
                        chat,
                        text,
                        replyMarkup: replyMarkup,
                        parseMode: ParseMode.Html
                    );
                }
                return message;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SendOrUpdate() Error for params: chat:{chat.Identifier}:{chat.Username}, text:{text}, isUpdate:{messageId.HasValue}");
                Console.WriteLine($"{ex.Message} - \n {ex.StackTrace}");
                return null;
            }
        }

        public Task SendNotification(ChatId chat, string text)
        {
            return SendOrUpdate(chat, text);
        }
    }
}
