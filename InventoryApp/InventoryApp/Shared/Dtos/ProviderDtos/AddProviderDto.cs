using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Shared.Dtos.ProviderDtos
{
    /// <summary>
    /// Dto for adding new provider into database
    /// </summary>
    public partial class AddProviderDto
    {
        [Required]
        [Column("name")]
        [StringLength(250)]
        [Unicode(false)]
        public string Name { get; set; } = null!;
        [Required, Phone]
        [Column("phone_number")]
        [StringLength(15)]
        [Unicode(false)]
        public string PhoneNumber { get; set; } = null!;
        [Required]
        [Column("address")]
        [StringLength(120)]
        [Unicode(false)]
        public string Address { get; set; } = null!;
        [Required]
        [Column("description", TypeName = "text")]
        public string? Description { get; set; }
    }
}