using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InventoryApp.Shared.Dtos.ProductDtos;

namespace InventoryApp.Shared.Dtos.SupplyDetailDtos
{
    /// <summary>
    /// Dto for updating supply detail
    /// </summary>
    public partial class AddSupplyDetailDto
    {
        [Required, Range(1, int.MaxValue, ErrorMessage = "Please enter a valid product")]
        [Column("id_product")]
        public int IdProduct { get; set; }
        [Required, Range(1, int.MaxValue, ErrorMessage = "Please enter a valid quantity")]
        [Column("quantity")]
        public int Quantity { get; set; }
        [Required, DataType(DataType.Currency)]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        [Column("unit_price", TypeName = "money")]
        public decimal UnitPrice { get; set; }
    }
}