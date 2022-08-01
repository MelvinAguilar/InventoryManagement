using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Shared.Models
{
    [Table("SUPPLY")]
    public partial class Supply
    {
        public Supply()
        {
            SupplyDetails = new HashSet<SupplyDetail>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("total_amount", TypeName = "money")]
        public decimal TotalAmount { get; set; }
        [Column("payment", TypeName = "money")]
        public decimal Payment { get; set; }
        [Column("date_supplied", TypeName = "datetime")]
        public DateTime DateSupplied { get; set; }
        [Column("date_modified", TypeName = "datetime")]
        public DateTime? DateModified { get; set; }
        [Column("id_employee")]
        public int IdEmployee { get; set; }
        [Column("id_provider")]
        public int IdProvider { get; set; }

        [ForeignKey("IdEmployee")]
        [InverseProperty("Supplies")]
        public virtual Employee IdEmployeeNavigation { get; set; } = null!;
        [ForeignKey("IdProvider")]
        [InverseProperty("Supplies")]
        public virtual Provider IdProviderNavigation { get; set; } = null!;

        [JsonIgnore]
        [InverseProperty("IdSupplyNavigation")]
        public virtual ICollection<SupplyDetail> SupplyDetails { get; set; }
    }
}
