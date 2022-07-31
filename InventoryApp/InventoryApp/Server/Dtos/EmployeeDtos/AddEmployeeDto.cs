using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

/*
    TODO: Implement the JWT token for the employee.
*/
namespace InventoryApp.Server.Dtos.EmployeeDtos
{
    /// <summary>
    /// Dto for adding new employee into database
    /// </summary>
    public partial class AddEmployeeDto
    {
        [Column("first_name")]
        [StringLength(50)]
        [Unicode(false)]
        public string FirstName { get; set; } = null!;
        [Column("last_name")]
        [StringLength(50)]
        [Unicode(false)]
        public string LastName { get; set; } = null!;
        [Column("email")]
        [StringLength(256)]
        [Unicode(false)]
        public string Email { get; set; } = null!;
        [Column("phone_number")]
        [StringLength(15)]
        [Unicode(false)]
        public string PhoneNumber { get; set; } = null!;
        [Column("avatar")]
        public byte[]? Avatar { get; set; }
        [Column("password")]
        [StringLength(256)]
        [Unicode(false)]
        public string Password { get; set; } = null!;  
        [Column("id_rol")]
        public int IdRol { get; set; }
    }
}