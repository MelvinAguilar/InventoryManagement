using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Shared.Models
{
    [Table("EMPLOYEE")]
    public partial class Employee
    {
        public Employee()
        {
            Purchases = new HashSet<Purchase>();
            Supplies = new HashSet<Supply>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("first_name")]
        [StringLength(50)]
        [Unicode(false)]
        public string FirstName { get; set; } = null!;
        [Column("last_name")]
        [StringLength(50)]
        [Unicode(false)]
        public string LastName { get; set; } = null!;
        [Column("email")]
        [StringLength(256)]
        [Unicode(false)]
        public string Email { get; set; } = null!;
        [Column("phone_number")]
        [StringLength(15)]
        [Unicode(false)]
        public string PhoneNumber { get; set; } = null!;
        [Column("avatar")]
        public byte[]? Avatar { get; set; }
        [Column("password")]
        [StringLength(256)]
        [Unicode(false)]
        public string Password { get; set; } = null!;
        [Column("date_created", TypeName = "datetime")]
        public DateTime DateCreated { get; set; }
        [Column("date_modified", TypeName = "datetime")]
        public DateTime? DateModified { get; set; }
        [Column("id_rol")]
        public int IdRol { get; set; }

        [ForeignKey("IdRol")]
        [InverseProperty("Employees")]
        public virtual Rol IdRolNavigation { get; set; } = null!;
        [InverseProperty("IdEmployeeNavigation")]
        public virtual ICollection<Purchase> Purchases { get; set; }
        [InverseProperty("IdEmployeeNavigation")]
        public virtual ICollection<Supply> Supplies { get; set; }
    }
}
