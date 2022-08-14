using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InventoryApp.Shared.Dtos.ProductDtos;

namespace InventoryApp.Shared.Dtos.PurchaseDetailDtos
{
    /// <summary>
    /// Dto for getting purchase detail
    /// </summary>
    public partial class GetPurchaseDetailDto
    {
        [Column("id_purchase")]
        public int IdPurchase { get; set; }
        [Column("id_product")]
        public int IdProduct { get; set; }
        [Column("quantity")]
        [Required, Range(1, int.MaxValue, ErrorMessage = "Please enter a valid quantity")]
        public int Quantity { get; set; }
        [Column("unit_price", TypeName = "money")]
        
        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal UnitPrice { get; set; }

        [ForeignKey("IdProduct")]
        [InverseProperty("PurchaseDetails")]
        public virtual GetProductDto IdProductNavigation { get; set; } = null!;
    }
}