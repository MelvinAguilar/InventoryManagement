using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Shared.Models
{
    [Table("SUPPLY_DETAILS")]
    public partial class SupplyDetail
    {
        [Key]
        [Column("id_supply")]
        public int IdSupply { get; set; }
        [Key]
        [Column("id_product")]
        public int IdProduct { get; set; }
        [Column("quantity")]
        public int Quantity { get; set; }
        [Column("unit_price", TypeName = "money")]
        public decimal UnitPrice { get; set; }

        [ForeignKey("IdProduct")]
        [InverseProperty("SupplyDetails")]
        public virtual Product IdProductNavigation { get; set; } = null!;
        [ForeignKey("IdSupply")]
        [InverseProperty("SupplyDetails")]
        public virtual Supply IdSupplyNavigation { get; set; } = null!;
    }
}
