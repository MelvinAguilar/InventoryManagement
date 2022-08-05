using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InventoryApp.Server.Dtos.SupplyDetailDtos;

namespace InventoryApp.Server.Dtos.SupplyDtos
{
    /// <summary>
    /// Dto for adding supply
    /// </summary>
    public partial class AddSupplyDto
    {
        public AddSupplyDto()
        {
            SupplyDetails = new HashSet<AddSupplyDetailDto>();
        }

        [Column("total_amount", TypeName = "money")]
        public decimal TotalAmount { get; set; }
        [Column("payment", TypeName = "money")]
        public decimal Payment { get; set; }
        [Column("date_supplied", TypeName = "datetime")]
        public DateTime DateSupplied { get; set; } = DateTime.Now;
        [Column("id_provider")]
        public int IdProvider { get; set; }

        [InverseProperty("IdSupplyNavigation")]
        public virtual ICollection<AddSupplyDetailDto> SupplyDetails { get; set; }
    }
}