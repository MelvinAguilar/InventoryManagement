using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Shared.Models
{
    [Table("PROVIDER")]
    [Index("Name", Name = "UQ__PROVIDER__72E12F1BD37C6E62", IsUnique = true)]
    [Index("PhoneNumber", Name = "UQ__PROVIDER__A1936A6B12B5D698", IsUnique = true)]
    public partial class Provider
    {
        public Provider()
        {
            Supplies = new HashSet<Supply>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        [StringLength(250)]
        [Unicode(false)]
        public string Name { get; set; } = null!;
        [Column("phone_number")]
        [StringLength(15)]
        [Unicode(false)]
        public string PhoneNumber { get; set; } = null!;
        [Column("address")]
        [StringLength(120)]
        [Unicode(false)]
        public string Address { get; set; } = null!;
        [Column("description", TypeName = "text")]
        public string? Description { get; set; }
        [Column("date_created", TypeName = "datetime")]
        public DateTime DateCreated { get; set; }
        [Column("date_modified", TypeName = "datetime")]
        public DateTime? DateModified { get; set; }

        [JsonIgnore]
        [InverseProperty("IdProviderNavigation")]
        public virtual ICollection<Supply> Supplies { get; set; }
    }
}
