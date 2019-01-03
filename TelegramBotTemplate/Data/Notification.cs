using System;

namespace TelegramBotTemplate.Data
{
    public class Notification
    {
        public int NotificationId { get; set; }

        public long ChatFromId { get; set; }
        public ChatUser ChatFrom { get; set; }

        public long ChatToId { get; set; }
        public ChatUser ChatTo { get; set; }

        public DateTime Created { get; set; }

        public DateTime? SendingStarted { get; set; }

        public DateTime? SendingFinished { get; set; }
    }
}
