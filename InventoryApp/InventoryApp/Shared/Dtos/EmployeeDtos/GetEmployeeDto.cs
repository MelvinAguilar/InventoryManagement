using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryApp.Shared.Dtos.EmployeeDtos
{
    /// <summary>
    /// Dto for getting employee
    /// </summary>
    public partial class GetEmployeeDto
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("first_name")]
        public string FirstName { get; set; } = null!;
        [Column("last_name")]
        public string LastName { get; set; } = null!;
        [Column("email")]
        public string Email { get; set; } = null!;
        [Column("phone_number")]
        public string PhoneNumber { get; set; } = null!;
        [Column("avatar")]
        public string Avatar { get; set; } = null!;
    }
}