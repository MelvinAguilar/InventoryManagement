using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Shared.Dtos.EmployeeDtos
{
    /// <summary>
    /// Dto for adding new employee into database
    /// </summary>
    public partial class AddEmployeeDto
    {
        [Required]
        [Column("first_name")]
        [StringLength(50)]
        [Unicode(false)]
        public string FirstName { get; set; } = null!;
        [Required]
        [Column("last_name")]
        [StringLength(50)]
        [Unicode(false)]
        public string LastName { get; set; } = null!;
        [Required, EmailAddress]
        [Column("email")]
        [StringLength(256)]
        [Unicode(false)]
        public string Email { get; set; } = null!;
        [Required, Phone]
        [Column("phone_number")]
        [StringLength(15)]
        [Unicode(false)]
        public string PhoneNumber { get; set; } = null!;
        [Required]
        [Column("avatar")]
        public byte[]? Avatar { get; set; }
        [Required, MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [NotMapped]
        [Column("password")]
        [StringLength(256)]
        [Unicode(false)]
        public string Password { get; set; } = null!;
        [Required]
        [Column("id_role")]
        public int IdRole { get; set; }
    }
}