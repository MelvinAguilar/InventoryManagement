using System.ComponentModel.DataAnnotations.Schema;
using InventoryApp.Shared.Dtos.ProductDtos;

namespace InventoryApp.Shared.Dtos.PurchaseDetailDtos
{
    /// <summary>
    /// Dto for adding purchase detail
    /// </summary>
    public partial class UpdatePurchaseDetailDto
    {
        [Column("quantity")]
        public int Quantity { get; set; }
        [Column("unit_price", TypeName = "money")]
        public decimal UnitPrice { get; set; }
        [Column("id_product")]
        public int IdProduct { get; set; }
    }
}