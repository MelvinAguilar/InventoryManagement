using System.ComponentModel.DataAnnotations.Schema;
using InventoryApp.Server.Dtos.CustomerDtos;
using InventoryApp.Server.Dtos.EmployeeDtos;
using InventoryApp.Server.Dtos.PurchaseDetailDtos;

namespace InventoryApp.Server.Dtos.PurchaseDtos
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