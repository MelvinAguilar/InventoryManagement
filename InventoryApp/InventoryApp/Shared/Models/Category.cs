using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Shared.Models
{
    [Table("CATEGORY")]
    [Index("Name", Name = "UQ__CATEGORY__72E12F1B6FAA9EF0", IsUnique = true)]
    public partial class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        [StringLength(250)]
        [Unicode(false)]
        public string Name { get; set; } = null!;
        [Column("description", TypeName = "text")]
        public string Description { get; set; } = null!;
        [Column("date_created", TypeName = "datetime")]
        public DateTime DateCreated { get; set; }
        [Column("date_modified", TypeName = "datetime")]
        public DateTime? DateModified { get; set; }

        [JsonIgnore]
        [InverseProperty("IdCategoryNavigation")]
        public virtual ICollection<Product> Products { get; set; }
    }
}
