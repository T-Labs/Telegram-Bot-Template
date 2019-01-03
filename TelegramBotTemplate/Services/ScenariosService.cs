using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TelegramBotTemplate.Data;
using TelegramBotTemplate.Services.Interfaces;

namespace TelegramBotTemplate.Services
{
    public class ScenariosService : IScenariosService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMessageSender _messageSender;
        private readonly INotificationQueue _notificationQueue;

        public ScenariosService(
            IConfiguration config,
            IMessageSender messageSender,
            INotificationQueue notificationQueue
           )
        {
            _messageSender = messageSender;
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseNpgsql(config.GetConnectionString("DefaultConnection"));
            _context = new ApplicationDbContext(optionsBuilder.Options);
            _notificationQueue = notificationQueue;
        }

        public Task SendErrorUnknownCommand(Chat chat)
        {
            return _messageSender.SendErrorUnknownCommand(chat);
        }

        public async Task StartCommand(Message message)
        {
            ChatUser chatUser = await CheckAndCreateUser(message);
        }

        public async Task TextCommand(Message message)
        {
            // check coordinates
            var ChatUser = await _context.ChatUsers.FirstAsync(u => u.ChatUserId == message.Chat.Id);

            // check or create bans
        }

        private async Task SaveMailingAsync(Message message)
        {
            var notification = new Notification();

            _notificationQueue.Enqueue(notification.NotificationId);
        }

        private async Task<ChatUser> CheckAndCreateUser(Message message)
        {
            var chatUser = await _context.GetChatUser(message.Chat.Id);

            if (chatUser == null)
            {
                chatUser = new ChatUser
                {
                    ChatUserId = message.Chat.Id,
                };

                chatUser.Username = message.Chat.Username ?? "";
                chatUser.Name = message.Chat.FirstName ?? "";

                if (!string.IsNullOrWhiteSpace(message.Chat.LastName))
                {
                    chatUser.Name += " " + message.Chat.LastName;
                }

                await _context.ChatUsers.AddAsync(chatUser);
                await _context.SaveChangesAsync();
            }

            return chatUser;
        }

        public async Task SendNotification(int id)
        {
            var notification = await _context.Notifications.Include(n => n.ChatTo).FirstAsync(n => n.NotificationId == id);
            var sendername = notification.ChatFromId.ToString();
            sendername = sendername.Substring(sendername.Length - 3);

            var text = "";

            // mark
            notification.SendingStarted = DateTime.Now;
            await _context.SaveChangesAsync();

            // send
            try
            {
                await _messageSender.SendNotification(notification.ChatToId, text);
                notification.SendingFinished = DateTime.Now;
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                Console.WriteLine($"Notification error, id: {notification.NotificationId}");
            }
        }
    }
}
