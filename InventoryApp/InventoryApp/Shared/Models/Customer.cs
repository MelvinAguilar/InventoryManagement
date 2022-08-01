using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Shared.Models
{
    [Table("CUSTOMER")]
    [Index("PhoneNumber", Name = "UQ__CUSTOMER__A1936A6B77803973", IsUnique = true)]
    public partial class Customer
    {
        public Customer()
        {
            Purchases = new HashSet<Purchase>();
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
        [Column("phone_number")]
        [StringLength(15)]
        [Unicode(false)]
        public string PhoneNumber { get; set; } = null!;
        [Column("date_created", TypeName = "datetime")]
        public DateTime DateCreated { get; set; }
        [Column("date_modified", TypeName = "datetime")]
        public DateTime? DateModified { get; set; }

        [JsonIgnore]
        [InverseProperty("IdCustomerNavigation")]
        public virtual ICollection<Purchase> Purchases { get; set; }
    }
}
