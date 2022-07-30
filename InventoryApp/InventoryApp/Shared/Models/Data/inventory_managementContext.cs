using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace InventoryApp.Shared.Models.Data
{
    public partial class inventory_managementContext : DbContext
    {
        public inventory_managementContext()
        {
        }

        public inventory_managementContext(DbContextOptions<inventory_managementContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<Employee> Employees { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<Provider> Providers { get; set; } = null!;
        public virtual DbSet<Purchase> Purchases { get; set; } = null!;
        public virtual DbSet<PurchaseDetail> PurchaseDetails { get; set; } = null!;
        public virtual DbSet<Rol> Rols { get; set; } = null!;
        public virtual DbSet<Supply> Supplies { get; set; } = null!;
        public virtual DbSet<SupplyDetail> SupplyDetails { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=localhost; Database=inventory_management; Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.IdRolNavigation)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.IdRol)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_rol_employee");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.IdCategoryNavigation)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.IdCategory)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_category_product");
            });

            modelBuilder.Entity<Provider>(entity =>
            {
                entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Purchase>(entity =>
            {
                entity.Property(e => e.DatePurchased).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Discount).HasDefaultValueSql("((0.00))");

                entity.Property(e => e.Tax).HasDefaultValueSql("((13.00))");

                entity.HasOne(d => d.IdCustomerNavigation)
                    .WithMany(p => p.Purchases)
                    .HasForeignKey(d => d.IdCustomer)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_customer_purchase");

                entity.HasOne(d => d.IdEmployeeNavigation)
                    .WithMany(p => p.Purchases)
                    .HasForeignKey(d => d.IdEmployee)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_employee_purchase");
            });

            modelBuilder.Entity<PurchaseDetail>(entity =>
            {
                entity.HasOne(d => d.IdProductNavigation)
                    .WithMany(p => p.PurchaseDetails)
                    .HasForeignKey(d => d.IdProduct)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_product_pdetail");

                entity.HasOne(d => d.IdPurchaseNavigation)
                    .WithMany(p => p.PurchaseDetails)
                    .HasForeignKey(d => d.IdPurchase)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_purchase_pdetail");
            });

            modelBuilder.Entity<Supply>(entity =>
            {
                entity.Property(e => e.DateSupplied).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.IdEmployeeNavigation)
                    .WithMany(p => p.Supplies)
                    .HasForeignKey(d => d.IdEmployee)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_employee_supply");

                entity.HasOne(d => d.IdProviderNavigation)
                    .WithMany(p => p.Supplies)
                    .HasForeignKey(d => d.IdProvider)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_provider_supply");
            });

            modelBuilder.Entity<SupplyDetail>(entity =>
            {
                entity.HasOne(d => d.IdProductNavigation)
                    .WithMany(p => p.SupplyDetails)
                    .HasForeignKey(d => d.IdProduct)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_product_sdetail");

                entity.HasOne(d => d.IdSupplyNavigation)
                    .WithMany(p => p.SupplyDetails)
                    .HasForeignKey(d => d.IdSupply)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_supply_sdetail");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
