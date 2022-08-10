using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InventoryApp.Shared.Dtos.SupplyDetailDtos;

namespace InventoryApp.Shared.Dtos.SupplyDtos
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
        [Required, DataType(DataType.Currency)]
        [Range(0, double.MaxValue, ErrorMessage = "Net amount must be greater than 0")]
        [Column("total_amount", TypeName = "money")]
        public decimal TotalAmount { get; set; }
        [Required, DataType(DataType.Currency)]
        [Range(0, double.MaxValue, ErrorMessage = "Net amount must be greater than 0")]
        [Column("payment", TypeName = "money")]
        public decimal Payment { get; set; }
        [Column("date_supplied", TypeName = "datetime")]
        public DateTime DateSupplied { get; set; } = DateTime.Now;
        [Required, Range(1, int.MaxValue, ErrorMessage = "Please enter a valid provider")]
        [Column("id_provider")]
        public int IdProvider { get; set; }

        [InverseProperty("IdSupplyNavigation")]
        public virtual ICollection<AddSupplyDetailDto> SupplyDetails { get; set; }
    }
}