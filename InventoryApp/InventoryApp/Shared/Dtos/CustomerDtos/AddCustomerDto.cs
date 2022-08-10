using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Shared.Dtos.CustomerDtos
{
    /// <summary>
    /// Dto for adding new customer into database
    /// </summary>
    public partial class AddCustomerDto
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
        [Required, Phone]
        [Column("phone_number")]
        [StringLength(15)]
        [Unicode(false)]
        public string PhoneNumber { get; set; } = null!;
    }
}