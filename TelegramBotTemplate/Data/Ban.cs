using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TelegramBotTemplate.Data
{
    public class Ban
    {
        public int BanId { get; set; }

        public ChatUser Chat { get; set; }

        [ForeignKey("Chat")]
        public long ChatId { get; set; }

        public DateTime Created { get; set; }

        public DateTime Expired { get; set; }
    }
}
