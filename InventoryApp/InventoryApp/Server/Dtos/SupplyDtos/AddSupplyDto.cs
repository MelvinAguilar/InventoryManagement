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
            SupplyDetails = new HashSet<GetSupplyDetailDto>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("total_amount", TypeName = "money")]
        public decimal TotalAmount { get; set; }
        [Column("payment", TypeName = "money")]
        public decimal Payment { get; set; }
        [Column("date_supplied", TypeName = "datetime")]
        public DateTime DateSupplied { get; set; }
        [Column("id_employee")]
        public int IdEmployee { get; set; }
        [Column("id_provider")]
        public int IdProvider { get; set; }

        [InverseProperty("IdSupplyNavigation")]
        public virtual ICollection<GetSupplyDetailDto> SupplyDetails { get; set; }
    }
}