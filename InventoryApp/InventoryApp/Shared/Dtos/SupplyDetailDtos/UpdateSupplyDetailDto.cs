using System.ComponentModel.DataAnnotations.Schema;
using InventoryApp.Shared.Dtos.ProductDtos;

namespace InventoryApp.Shared.Dtos.SupplyDetailDtos
{
    /// <summary>
    /// Dto for adding supply detail
    /// </summary>
    public partial class UpdateSupplyDetailDto
    {
        [Column("id_product")]
        public int IdProduct { get; set; }
        [Column("quantity")]
        public int Quantity { get; set; }
        [Column("unit_price", TypeName = "money")]
        public decimal UnitPrice { get; set; }
    }
}