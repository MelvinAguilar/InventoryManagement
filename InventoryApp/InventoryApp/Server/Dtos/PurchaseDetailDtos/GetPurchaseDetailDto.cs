using System.ComponentModel.DataAnnotations.Schema;
using InventoryApp.Server.Dtos.ProductDtos;

namespace InventoryApp.Server.Dtos.PurchaseDetailDtos
{
    /// <summary>
    /// Dto for getting purchase
    /// </summary>
    public partial class GetPurchaseDetailDto
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("quantity")]
        public int Quantity { get; set; }
        [Column("unit_price", TypeName = "money")]
        public decimal UnitPrice { get; set; }
        [Column("id_purchase")]
        public int IdPurchase { get; set; }
        [Column("id_product")]
        public int IdProduct { get; set; }

        [ForeignKey("IdProduct")]
        [InverseProperty("PurchaseDetails")]
        public virtual GetProductDto IdProductNavigation { get; set; } = null!;
    }
}