using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InventoryApp.Server.Dtos.PurchaseDetailDtos;

namespace InventoryApp.Server.Dtos.PurchaseDtos
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

        [Column("gross_amount", TypeName = "money")]
        public decimal GrossAmount { get; set; }
        [Column("tax", TypeName = "decimal(5, 2)")]
        public decimal Tax { get; set; }
        [Column("discount", TypeName = "decimal(5, 2)")]
        public decimal Discount { get; set; }
        [Column("net_amount", TypeName = "money")]
        public decimal NetAmount { get; set; }
        [Column("payment", TypeName = "money")]
        public decimal Payment { get; set; }
        [Column("date_purchased", TypeName = "datetime")]
        public DateTime DatePurchased { get; set; } = DateTime.Now;
        [Column("id_customer")]
        public int IdCustomer { get; set; }

        [InverseProperty("IdPurchaseNavigation")]
        public virtual ICollection<AddPurchaseDetailDto> PurchaseDetails { get; set; }
    }
}