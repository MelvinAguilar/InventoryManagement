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
        [Required]
        [Column("name")]
        [StringLength(150)]
        [Unicode(false)]
        public string Name { get; set; } = null!;
        [Required]
        [Column("specification")]
        [StringLength(250)]
        [Unicode(false)]
        public string Specification { get; set; } = null!;
        [Required]
        [Column("brand")]
        [StringLength(150)]
        [Unicode(false)]
        public string Brand { get; set; } = null!;
        [Required, Range(1, int.MaxValue, ErrorMessage = "Stock must be greater than 0")]
        [Column("stock")]
        public int Stock { get; set; }
        [Required, DataType(DataType.Currency)]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        [Column("price", TypeName = "money")]
        public decimal Price { get; set; }
        [Required]
        [Column("image")]
        public string Image { get; set; } = null!;
        [Required, Range(1, int.MaxValue, ErrorMessage = "Please enter a valid category")]
        [Column("id_category")]
        public int IdCategory { get; set; }
    }
}