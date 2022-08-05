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
        [Column("name")]
        [StringLength(250)]
        [Unicode(false)]
        public string Name { get; set; } = null!;
        [Column("phone_number")]
        [StringLength(15)]
        [Unicode(false)]
        public string PhoneNumber { get; set; } = null!;
        [Column("address")]
        [StringLength(120)]
        [Unicode(false)]
        public string Address { get; set; } = null!;
        [Column("description", TypeName = "text")]
        public string? Description { get; set; }
    }
}