using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Server.Dtos.CustomerDtos
{
    /// <summary>
    /// Dto for adding new customer into database
    /// </summary>
    public partial class AddCustomerDto
    {
        [Column("first_name")]
        [StringLength(50)]
        [Unicode(false)]
        public string FirstName { get; set; } = null!;
        [Column("last_name")]
        [StringLength(50)]
        [Unicode(false)]
        public string LastName { get; set; } = null!;
        [Column("phone_number")]
        [StringLength(15)]
        [Unicode(false)]
        public string PhoneNumber { get; set; } = null!;
    }
}