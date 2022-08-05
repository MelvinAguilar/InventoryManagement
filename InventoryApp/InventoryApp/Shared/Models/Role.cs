using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Shared.Models
{
    [Table("ROLE")]
    [Index("Name", Name = "UQ__ROLE__72E12F1BE1DAE22C", IsUnique = true)]
    public partial class Role
    {
        public Role()
        {
            Employees = new HashSet<Employee>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        [StringLength(30)]
        [Unicode(false)]
        public string Name { get; set; } = null!;

        [JsonIgnore]
        [InverseProperty("IdRoleNavigation")]
        public virtual ICollection<Employee> Employees { get; set; }
    }
}
