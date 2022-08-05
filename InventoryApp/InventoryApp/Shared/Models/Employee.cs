using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Shared.Models
{
    [Table("EMPLOYEE")]
    [Index("PhoneNumber", Name = "UQ__EMPLOYEE__A1936A6BCDAF41BF", IsUnique = true)]
    [Index("Email", Name = "UQ__EMPLOYEE__AB6E6164523DC6B6", IsUnique = true)]
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
        [Column("passwordHash")]
        public byte[] PasswordHash { get; set; } = null!;
        [Column("passwordSalt")]
        public byte[] PasswordSalt { get; set; } = null!;
        [Column("verificationToken")]
        [StringLength(256)]
        [Unicode(false)]
        public string? VerificationToken { get; set; }
        [Column("verifiedAt", TypeName = "datetime")]
        public DateTime? VerifiedAt { get; set; }
        [Column("passwordResetToken")]
        [StringLength(256)]
        [Unicode(false)]
        public string? PasswordResetToken { get; set; }
        [Column("resetTokenExpires", TypeName = "datetime")]
        public DateTime? ResetTokenExpires { get; set; }
        [Column("date_created", TypeName = "datetime")]
        public DateTime DateCreated { get; set; }
        [Column("date_modified", TypeName = "datetime")]
        public DateTime? DateModified { get; set; }
        [Column("id_role")]
        public int IdRole { get; set; }

        [ForeignKey("IdRole")]
        [InverseProperty("Employees")]
        public virtual Role IdRoleNavigation { get; set; } = null!;
        [JsonIgnore]
        [InverseProperty("IdEmployeeNavigation")]
        public virtual ICollection<Purchase> Purchases { get; set; }
        [JsonIgnore]
        [InverseProperty("IdEmployeeNavigation")]
        public virtual ICollection<Supply> Supplies { get; set; }
    }
}
