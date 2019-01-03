using System.Threading;
using System.Threading.Tasks;

namespace TelegramBotTemplate.Services.Interfaces
{
    public interface INotificationQueue
    {
        void Enqueue(int id);

        Task<int> DequeueAsync(CancellationToken cancellationToken);
    }
}
