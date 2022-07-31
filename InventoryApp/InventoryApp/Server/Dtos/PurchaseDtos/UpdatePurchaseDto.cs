using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryApp.Server.Dtos.PurchaseDtos
{
    /// <summary>
    /// Dto for updating purchase
    /// </summary>
    public partial class UpdatePurchaseDto
    {
        [Key]
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
        [Column("id_employee")]
        public int IdEmployee { get; set; }
    }
}