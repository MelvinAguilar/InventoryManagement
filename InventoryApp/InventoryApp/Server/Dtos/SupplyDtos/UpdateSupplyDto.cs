using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InventoryApp.Server.Dtos.SupplyDetailDtos;

namespace InventoryApp.Server.Dtos.SupplyDtos
{
    /// <summary>
    /// Dto for updating supply
    /// </summary>
    public partial class UpdateSupplyDto
    {
        public UpdateSupplyDto()
        {
            SupplyDetails = new HashSet<UpdateSupplyDetailDto>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("total_amount", TypeName = "money")]
        public decimal TotalAmount { get; set; }
        [Column("payment", TypeName = "money")]
        public decimal Payment { get; set; }
        [Column("id_employee")]
        public int IdEmployee { get; set; }

        [InverseProperty("IdSupplyNavigation")]
        public virtual ICollection<UpdateSupplyDetailDto> SupplyDetails { get; set; }
    }
}