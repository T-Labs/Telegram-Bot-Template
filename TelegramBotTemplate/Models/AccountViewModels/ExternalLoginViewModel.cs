using System.ComponentModel.DataAnnotations;

namespace TelegramBotTemplate.Models.AccountViewModels
{
    public class ExternalLoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
