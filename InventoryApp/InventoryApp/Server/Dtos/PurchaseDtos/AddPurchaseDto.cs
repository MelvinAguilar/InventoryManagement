using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryApp.Server.Dtos.PurchaseDtos
{
    /// <summary>
    /// Dto for adding purchase
    /// </summary>
    public partial class AddPurchaseDto
    {
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
    }
}