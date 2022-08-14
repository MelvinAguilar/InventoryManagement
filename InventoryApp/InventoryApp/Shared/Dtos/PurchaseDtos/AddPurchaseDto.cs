using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InventoryApp.Shared.Dtos.PurchaseDetailDtos;

namespace InventoryApp.Shared.Dtos.PurchaseDtos
{
    /// <summary>
    /// Dto for adding purchase
    /// </summary>
    public partial class AddPurchaseDto
    {
        public AddPurchaseDto()
        {
            PurchaseDetails = new HashSet<AddPurchaseDetailDto>();
        }

        [Required]  
        [Column("gross_amount", TypeName = "money")]
        public decimal GrossAmount { get; set; }
        [Range(0, 100, ErrorMessage = "Tax must be between 0 and 100")]
        [Column("tax", TypeName = "decimal(5, 2)")]
        public decimal Tax { get; set; }
        [Range(0, 100, ErrorMessage = "Discount must be between 0 and 100")]
        [Column("discount", TypeName = "decimal(5, 2)")]
        public decimal Discount { get; set; }
        [Required, DataType(DataType.Currency)]
        [Range(0, double.MaxValue, ErrorMessage = "Net amount must be greater than 0")]
        [Column("net_amount", TypeName = "money")]
        public decimal NetAmount { get; set; }
        [Required, DataType(DataType.Currency)]
        [Range(0, double.MaxValue, ErrorMessage = "Payment must be greater than 0")]
        [Column("payment", TypeName = "money")]
        public decimal Payment { get; set; }
        [Column("date_purchased", TypeName = "datetime")]
        public DateTime DatePurchased { get; set; } = DateTime.Now;
        [Required, Range(1, int.MaxValue, ErrorMessage = "Please enter a valid customer")]
        [Column("id_customer")]
        public int IdCustomer { get; set; }

        [InverseProperty("IdPurchaseNavigation")]
        public virtual ICollection<AddPurchaseDetailDto> PurchaseDetails { get; set; }
    }
}