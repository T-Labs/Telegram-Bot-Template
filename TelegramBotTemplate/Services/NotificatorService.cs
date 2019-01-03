using TelegramBotTemplate.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TelegramBotTemplate.Services
{
    public class NotificatorService : IHostedService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly INotificationQueue _notificationQueue;
        private Task _dequeueNotificationTask;
        private readonly CancellationTokenSource _stopTokenSource = new CancellationTokenSource();

        public NotificatorService(
            IServiceScopeFactory scopeFactory,
            INotificationQueue notificationQueue
            )
        {
            _scopeFactory = scopeFactory;
            _notificationQueue = notificationQueue;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // put notifications from database to queue

            _dequeueNotificationTask = Task.Run(DequeueNotificationTask);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _stopTokenSource.Cancel();
            return Task.WhenAny(_dequeueNotificationTask, Task.Delay(Timeout.Infinite, cancellationToken));
        }

        private async Task DequeueNotificationTask()
        {
            var semaphore = new SemaphoreSlim(Environment.ProcessorCount);

            void HandleTask(Task task)
            {
                semaphore.Release();
            }

            while (!_stopTokenSource.IsCancellationRequested)
            {
                await semaphore.WaitAsync();
                var item = await _notificationQueue.DequeueAsync(_stopTokenSource.Token);
                using (var scope = _scopeFactory.CreateScope())
                {
                    var scenariosService = scope.ServiceProvider.GetRequiredService<IScenariosService>();
                    var task = scenariosService.SendNotification(item);
                    task.ContinueWith(HandleTask);
                }
            }
        }
    }
}
