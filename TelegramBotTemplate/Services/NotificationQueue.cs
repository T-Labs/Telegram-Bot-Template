using TelegramBotTemplate.Services.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace TelegramBotTemplate.Services
{
    public class NotificationQueue : INotificationQueue
    {
        private readonly ConcurrentQueue<int> _notifications = new ConcurrentQueue<int>();
        private readonly SemaphoreSlim _notificationsEnqueuedSignal = new SemaphoreSlim(0);

        public void Enqueue(int id)
        {
            Console.WriteLine($"Notification enqueue {id}");
            _notifications.Enqueue(id);
            _notificationsEnqueuedSignal.Release();
        }

        public async Task<int> DequeueAsync(CancellationToken cancellationToken)
        {
            await _notificationsEnqueuedSignal.WaitAsync(cancellationToken);
            _notifications.TryDequeue(out int id);
            Console.WriteLine($"Notification dequeue {id}");
            return id;
        }
    }
}
