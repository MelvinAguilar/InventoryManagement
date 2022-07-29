using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Shared.Models
{
    [Table("PURCHASE_DETAILS")]
    public partial class PurchaseDetail
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("quantity")]
        public int Quantity { get; set; }
        [Column("unit_price", TypeName = "money")]
        public decimal UnitPrice { get; set; }
        [Column("id_purchase")]
        public int IdPurchase { get; set; }
        [Column("id_product")]
        public int IdProduct { get; set; }

        [ForeignKey("IdProduct")]
        [InverseProperty("PurchaseDetails")]
        public virtual Product IdProductNavigation { get; set; } = null!;
        [ForeignKey("IdPurchase")]
        [InverseProperty("PurchaseDetails")]
        public virtual Purchase IdPurchaseNavigation { get; set; } = null!;
    }
}
