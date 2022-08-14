using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InventoryApp.Shared.Dtos.ProductDtos;

namespace InventoryApp.Shared.Dtos.SupplyDetailDtos
{
    /// <summary>
    /// Dto for getting supply detail
    /// </summary>
    public partial class GetSupplyDetailDto
    {
        [Key]
        [Column("id_supply")]
        public int IdSupply { get; set; }
        [Key]
        [Column("id_product")]
        public int IdProduct { get; set; }
        [Column("quantity")]
        [Required, Range(1, int.MaxValue, ErrorMessage = "Please enter a valid quantity")]
        public int Quantity { get; set; }
        [Column("unit_price", TypeName = "money")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal UnitPrice { get; set; }

        [ForeignKey("IdProduct")]
        [InverseProperty("SupplyDetails")]
        public virtual GetProductDto IdProductNavigation { get; set; } = null!;
    }
}