using System.ComponentModel.DataAnnotations.Schema;
using InventoryApp.Shared.Dtos.CustomerDtos;
using InventoryApp.Shared.Dtos.EmployeeDtos;
using InventoryApp.Shared.Dtos.PurchaseDetailDtos;

namespace InventoryApp.Shared.Dtos.PurchaseDtos
{
    /// <summary>
    /// Dto for getting purchase
    /// </summary>
    public partial class GetPurchaseDto
    {
        public GetPurchaseDto()
        {
            PurchaseDetails = new HashSet<GetPurchaseDetailDto>();
        }

        [Column("id")]
        public int Id { get; set; }
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
        public DateTime DatePurchased { get; set; }
        [Column("id_employee")]
        public int IdEmployee { get; set; }
        [Column("id_customer")]
        public int IdCustomer { get; set; }

        [ForeignKey("IdCustomer")]
        [InverseProperty("Purchases")]
        public virtual GetCustomerDto IdCustomerNavigation { get; set; } = null!;
        [ForeignKey("IdEmployee")]
        [InverseProperty("Purchases")]
        public virtual GetEmployeeDto IdEmployeeNavigation { get; set; } = null!;

        [InverseProperty("IdPurchaseNavigation")]
        public virtual ICollection<GetPurchaseDetailDto> PurchaseDetails { get; set; }
    }
}