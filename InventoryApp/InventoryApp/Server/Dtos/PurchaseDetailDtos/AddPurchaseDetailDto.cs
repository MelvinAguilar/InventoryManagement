using System.ComponentModel.DataAnnotations.Schema;
using InventoryApp.Server.Dtos.ProductDtos;

namespace InventoryApp.Server.Dtos.PurchaseDetailDtos
{
    /// <summary>
    /// Dto for adding purchase detail
    /// </summary>
    public partial class AddPurchaseDetailDto
    {
        [Column("quantity")]
        public int Quantity { get; set; }
        [Column("unit_price", TypeName = "money")]
        public decimal UnitPrice { get; set; }
        [Column("id_product")]
        public int IdProduct { get; set; }
    }
}