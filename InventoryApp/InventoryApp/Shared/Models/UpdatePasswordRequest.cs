using System.ComponentModel.DataAnnotations;

namespace InventoryApp.Shared.Models
{
    public class UpdatePasswordRequest
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string OldPassword { get; set; } = null!;
        [Required, MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string NewPassword { get; set; } = null!;

    }
}