using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TelegramBotTemplate.Data
{
    public class ChatUser
    {
        [Key]
        public long ChatUserId { get; set; }

        public string Username { get; set; }

        public string Name { get; set; }

        public DateTime Created { get; set; }

        public List<Ban> Bans { get; set; }
    }
}
