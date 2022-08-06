using System.ComponentModel.DataAnnotations;

namespace InventoryApp.Shared.Models
{
    public class ForgotPasswordRequest
    {
        [Required]
        public string Email { get; set; } = null!;
    }
}