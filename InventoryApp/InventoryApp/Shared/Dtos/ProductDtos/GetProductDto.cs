using System.ComponentModel.DataAnnotations.Schema;
using InventoryApp.Shared.Dtos.CategoryDtos;

namespace InventoryApp.Shared.Dtos.ProductDtos
{
    /// <summary>
    /// Dto for getting product
    /// </summary>
    public partial class GetProductDto
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; } = null!;
        [Column("specification")]
        public string Specification { get; set; } = null!;
        [Column("brand")]
        public string Brand { get; set; } = null!;
        [Column("stock")]
        public int Stock { get; set; }
        [Column("price", TypeName = "money")]
        public decimal Price { get; set; }
        [Column("image")]
        public string Image { get; set; } = null!;
        [Column("id_category")]
        public int IdCategory { get; set; }
        [NotMapped]
        public int Quantity { get; set; }

        [ForeignKey("IdCategory")]
        [InverseProperty("Products")]
        public virtual GetCategoryDto IdCategoryNavigation { get; set; } = null!;
    }
}