using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TelegramBotTemplate.Models;

namespace TelegramBotTemplate.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<ChatUser> ChatUsers { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<Ban> Bans { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public async Task<ChatUser> GetChatUser(long chatId)
        {
            return await ChatUsers.FirstOrDefaultAsync(c => c.ChatUserId == chatId);
        }
    }
}
