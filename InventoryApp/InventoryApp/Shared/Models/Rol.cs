﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Shared.Models
{
    [Table("ROL")]
    [Index("Rol1", Name = "UQ__ROL__C2B79D269E51AF2C", IsUnique = true)]
    public partial class Rol
    {
        public Rol()
        {
            Employees = new HashSet<Employee>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("rol")]
        [StringLength(30)]
        [Unicode(false)]
        public string Rol1 { get; set; } = null!;

        [InverseProperty("IdRolNavigation")]
        public virtual ICollection<Employee> Employees { get; set; }
    }
}