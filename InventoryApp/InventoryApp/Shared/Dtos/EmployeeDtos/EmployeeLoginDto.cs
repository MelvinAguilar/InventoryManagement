using System.ComponentModel.DataAnnotations;

namespace InventoryApp.Shared.Dtos.EmployeeDtos
{
    public class EmployeeLoginDto
    {
        [Required, EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!; 
    }
}