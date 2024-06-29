using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using DbFirst.Models;

namespace DbFirst.Context
{
    public partial class APBD_6Context : DbContext
    {
        public APBD_6Context()
        {
        }

        public APBD_6Context(DbContextOptions<APBD_6Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<ProductWarehouse> ProductWarehouses { get; set; } = null!;
        public virtual DbSet<Warehouse> Warehouses { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\localDB1;Database=APBD_6;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.IdOrder)
                    .HasName("Order_pk");

                entity.ToTable("Order");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.FulfilledAt).HasColumnType("datetime");

                entity.HasOne(d => d.IdProductNavigation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.IdProduct)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Receipt_Product");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.IdProduct)
                    .HasName("Product_pk");

                entity.ToTable("Product");

                entity.Property(e => e.Description).HasMaxLength(200);

                entity.Property(e => e.Name).HasMaxLength(200);

                entity.Property(e => e.Price).HasColumnType("numeric(25, 2)");
            });

            modelBuilder.Entity<ProductWarehouse>(entity =>
            {
                entity.HasKey(e => e.IdProductWarehouse)
                    .HasName("Product_Warehouse_pk");

                entity.ToTable("Product_Warehouse");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.Price).HasColumnType("numeric(25, 2)");

                entity.HasOne(d => d.IdOrderNavigation)
                    .WithMany(p => p.ProductWarehouses)
                    .HasForeignKey(d => d.IdOrder)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Product_Warehouse_Order");

                entity.HasOne(d => d.IdProductNavigation)
                    .WithMany(p => p.ProductWarehouses)
                    .HasForeignKey(d => d.IdProduct)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("_Product");

                entity.HasOne(d => d.IdWarehouseNavigation)
                    .WithMany(p => p.ProductWarehouses)
                    .HasForeignKey(d => d.IdWarehouse)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("_Warehouse");
            });

            modelBuilder.Entity<Warehouse>(entity =>
            {
                entity.HasKey(e => e.IdWarehouse)
                    .HasName("Warehouse_pk");

                entity.ToTable("Warehouse");

                entity.Property(e => e.Address).HasMaxLength(200);

                entity.Property(e => e.Name).HasMaxLength(200);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
