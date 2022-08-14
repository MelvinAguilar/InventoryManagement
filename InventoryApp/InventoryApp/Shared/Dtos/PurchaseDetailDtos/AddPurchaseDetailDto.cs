using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryApp.Shared.Dtos.PurchaseDetailDtos
{
    /// <summary>
    /// Dto for adding purchase detail
    /// </summary>
    public partial class AddPurchaseDetailDto
    {
        [Required, Range(1, int.MaxValue, ErrorMessage = "Please enter a valid quantity")]
        [Column("quantity")]
        public int Quantity { get; set; }
        [Required, DataType(DataType.Currency)]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        [Column("unit_price", TypeName = "money")]
        public decimal UnitPrice { get; set; }
        [Required, Range(1, int.MaxValue, ErrorMessage = "Please enter a valid product")]
        [Column("id_product")]
        public int IdProduct { get; set; }
    }
}