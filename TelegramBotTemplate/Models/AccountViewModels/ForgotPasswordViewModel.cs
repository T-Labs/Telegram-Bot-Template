using System.ComponentModel.DataAnnotations;

namespace TelegramBotTemplate.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
