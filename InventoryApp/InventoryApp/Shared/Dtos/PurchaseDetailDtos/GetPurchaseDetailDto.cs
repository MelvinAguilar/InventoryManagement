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
        public int Quantity { get; set; }
        [Column("unit_price", TypeName = "money")]
        public decimal UnitPrice { get; set; }

        [ForeignKey("IdProduct")]
        [InverseProperty("PurchaseDetails")]
        public virtual GetProductDto IdProductNavigation { get; set; } = null!;
    }
}