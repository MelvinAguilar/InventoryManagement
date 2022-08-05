using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Shared.Dtos.ProductDtos
{
    /// <summary>
    /// Dto for adding new product into database
    /// </summary>
    public partial class AddProductDto
    {
        [Column("name")]
        [StringLength(150)]
        [Unicode(false)]
        public string Name { get; set; } = null!;
        [Column("specification")]
        [StringLength(250)]
        [Unicode(false)]
        public string Specification { get; set; } = null!;
        [Column("brand")]
        [StringLength(150)]
        [Unicode(false)]
        public string Brand { get; set; } = null!;
        [Column("stock")]
        public int Stock { get; set; }
        [Column("price", TypeName = "money")]
        public decimal Price { get; set; }
        [Column("image")]
        public byte[]? Image { get; set; }
        [Column("date_created", TypeName = "datetime")]
        public int IdCategory { get; set; }
    }
}