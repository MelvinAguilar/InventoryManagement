using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Shared.Models
{
    [Table("PRODUCT")]
    public partial class Product
    {
        public Product()
        {
            PurchaseDetails = new HashSet<PurchaseDetail>();
            SupplyDetails = new HashSet<SupplyDetail>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        [StringLength(150)]
        [Unicode(false)]
        public string Name { get; set; } = null!;
        [Column("specification")]
        [StringLength(250)]
        [Unicode(false)]
        public string Specification { get; set; } = null!;
        [Column("brand")]
        [StringLength(150)]
        [Unicode(false)]
        public string Brand { get; set; } = null!;
        [Column("stock")]
        public int Stock { get; set; }
        [Column("price", TypeName = "money")]
        public decimal Price { get; set; }
        [Column("image")]
        public string? Image { get; set; }
        [Column("date_created", TypeName = "datetime")]
        public DateTime DateCreated { get; set; }
        [Column("date_modified", TypeName = "datetime")]
        public DateTime? DateModified { get; set; }
        [Column("id_category")]
        public int IdCategory { get; set; }

        [ForeignKey("IdCategory")]
        [InverseProperty("Products")]
        public virtual Category IdCategoryNavigation { get; set; } = null!;
        [JsonIgnore]
        [InverseProperty("IdProductNavigation")]
        public virtual ICollection<PurchaseDetail> PurchaseDetails { get; set; }
        [JsonIgnore]
        [InverseProperty("IdProductNavigation")]
        public virtual ICollection<SupplyDetail> SupplyDetails { get; set; }
    }
}
