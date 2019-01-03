using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBotTemplate.Services.Interfaces
{
    public interface IMessageSender
    {
        Task SendErrorUnknownCommand(Chat chat);

        Task SendNotification(ChatId chat, string text);

        Task<Message> SendOrUpdate(ChatId chat, string text, IReplyMarkup replyMarkup = null, int? messageId = null);
    }
}
