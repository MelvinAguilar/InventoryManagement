using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Shared.Models
{
    [Table("PURCHASE")]
    public partial class Purchase
    {
        public Purchase()
        {
            PurchaseDetails = new HashSet<PurchaseDetail>();
        }

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
        [Column("date_purchased", TypeName = "datetime")]
        public DateTime DatePurchased { get; set; }
        [Column("date_modified", TypeName = "datetime")]
        public DateTime? DateModified { get; set; }
        [Column("id_employee")]
        public int IdEmployee { get; set; }
        [Column("id_customer")]
        public int IdCustomer { get; set; }

        [ForeignKey("IdCustomer")]
        [InverseProperty("Purchases")]
        public virtual Customer IdCustomerNavigation { get; set; } = null!;
        [ForeignKey("IdEmployee")]
        [InverseProperty("Purchases")]
        public virtual Employee IdEmployeeNavigation { get; set; } = null!;
        [JsonIgnore]
        [InverseProperty("IdPurchaseNavigation")]
        public virtual ICollection<PurchaseDetail> PurchaseDetails { get; set; }
    }
}
