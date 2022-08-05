using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InventoryApp.Server.Dtos.ProductDtos;

namespace InventoryApp.Server.Dtos.SupplyDetailDtos
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
        public int Quantity { get; set; }
        [Column("unit_price", TypeName = "money")]
        public decimal UnitPrice { get; set; }

        [ForeignKey("IdProduct")]
        [InverseProperty("SupplyDetails")]
        public virtual GetProductDto IdProductNavigation { get; set; } = null!;
    }
}