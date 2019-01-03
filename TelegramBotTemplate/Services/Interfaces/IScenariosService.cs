using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace TelegramBotTemplate.Services.Interfaces
{
    public interface IScenariosService
    {
        Task StartCommand(Message message);

        Task SendErrorUnknownCommand(Chat chat);

        Task TextCommand(Message message);

        Task SendNotification(int item);
    }
}
