using System.ComponentModel.DataAnnotations;

namespace InventoryApp.Shared.Models
{
    public class ResetPasswordRequest
    {
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        public string Token { get; set; } = null!;
        [Required, MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string Password { get; set; } = null!;
        [Required, Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = null!;
    }
}