﻿using System;
using System.Collections.Generic;
using API.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BranchEntity> Branches { get; set; }

    public virtual DbSet<CartEntity> Carts { get; set; }

    public virtual DbSet<CartItemEntity> CartItems { get; set; }

    public virtual DbSet<CatalogEntity> Catalogs { get; set; }

    public virtual DbSet<CatalogCategoryEntity> CatalogCategories { get; set; }

    public virtual DbSet<CategoryEntity> Categories { get; set; }

    public virtual DbSet<CategoryItemEntity> CategoryItems { get; set; }

    public virtual DbSet<CustomerEntity> Customers { get; set; }

    public virtual DbSet<ItemEntity> Items { get; set; }

    public virtual DbSet<LineOfCreditEntity> LineOfCredits { get; set; }

    public virtual DbSet<OrderEntity> Orders { get; set; }

    public virtual DbSet<OrderItemEntity> OrderItems { get; set; }

    public virtual DbSet<PackageEntity> Packages { get; set; }

    public virtual DbSet<PackageItemEntity> PackageItems { get; set; }

    public virtual DbSet<PermissionEntity> Permissions { get; set; }

    public virtual DbSet<RoleEntity> Roles { get; set; }

    public virtual DbSet<RolePermissionEntity> RolePermissions { get; set; }

    public virtual DbSet<SupplierEntity> Suppliers { get; set; }

    public virtual DbSet<SupplierItemEntity> SupplierItems { get; set; }

    public virtual DbSet<UserEntity> Users { get; set; }

    public virtual DbSet<UserRoleEntity> UserRoles { get; set; }

    public virtual DbSet<WarehouseEntity> Warehouses { get; set; }

    public virtual DbSet<WarehouseItemEntity> WarehouseItems { get; set; }

   
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BranchEntity>(entity =>
        {
            entity.HasKey(e => e.BranchId).HasName("PK__Branch__A1682FC500954FE5");

            entity.ToTable("Branch");

            entity.Property(e => e.BranchId).ValueGeneratedNever();
            entity.Property(e => e.Address).HasColumnType("text");
            entity.Property(e => e.City)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ContactEmail)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ContactNumber)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Region)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CartEntity>(entity =>
        {
            entity.HasKey(e => e.CartId).HasName("PK__Cart__51BCD7B7103B1C89");

            entity.ToTable("Cart");

            entity.Property(e => e.CartId).ValueGeneratedNever();

            entity.HasOne(d => d.Customer).WithMany(p => p.Carts)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_Cart_Customer");
        });

        modelBuilder.Entity<CartItemEntity>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("CartItem");

            entity.HasOne(d => d.Cart).WithMany()
                .HasForeignKey(d => d.CartId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CartItem_Cart");

            entity.HasOne(d => d.SkuNavigation).WithMany()
                .HasForeignKey(d => d.Sku)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CartItem_Item");
        });

        modelBuilder.Entity<CatalogEntity>(entity =>
        {
            entity.HasKey(e => e.CatalogId).HasName("PK__Catalog__C2513B6835A0C5A2");

            entity.ToTable("Catalog");

            entity.Property(e => e.CatalogId).ValueGeneratedNever();
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CatalogCategoryEntity>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("CatalogCategory");

            entity.HasOne(d => d.Catalog).WithMany()
                .HasForeignKey(d => d.CatalogId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CatalogCategory_Catalog");

            entity.HasOne(d => d.Category).WithMany()
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CatalogCategory_Category");
        });

        modelBuilder.Entity<CategoryEntity>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__19093A0B813139CC");

            entity.ToTable("Category");

            entity.Property(e => e.CategoryId).ValueGeneratedNever();
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CategoryItemEntity>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("CategoryItem");

            entity.HasOne(d => d.Category).WithMany()
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CategoryItem_Category");

            entity.HasOne(d => d.SkuNavigation).WithMany()
                .HasForeignKey(d => d.Sku)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CategoryItem_Item");
        });

        modelBuilder.Entity<CustomerEntity>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__A4AE64D892601F68");

            entity.ToTable("Customer");

            entity.Property(e => e.CustomerId).ValueGeneratedNever();
            entity.Property(e => e.CustomerType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ItemEntity>(entity =>
        {
            // 1. Define Primary Key
            entity.HasKey(e => e.Sku).HasName("PK__Item__CA1FD3C4297D061E");

            // 2. Define Relationships
            entity.HasOne(d => d.Category).WithMany(p => p.Items)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_Item_Category");

            // 3. Configure Properties (data types, lengths, etc.)
            entity.ToTable("Item");

            entity.Property(e => e.Sku).ValueGeneratedNever();
            entity.Property(e => e.AdditionalTax).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Currency)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Discount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Image).HasColumnType("text");
            entity.Property(e => e.ItemPrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.MarginGain).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UnitCost).HasColumnType("decimal(10, 2)");
        });


        modelBuilder.Entity<LineOfCreditEntity>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__LineOfCr__A4AE64D8236E36B3");

            entity.ToTable("LineOfCredit");

            entity.Property(e => e.CustomerId).ValueGeneratedNever();
            entity.Property(e => e.CreditLimit).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.CurrentBalance).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Customer).WithOne(p => p.LineOfCredit)
                .HasForeignKey<LineOfCreditEntity>(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LineOfCredit_Customer");
        });

        modelBuilder.Entity<OrderEntity>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Order__C3905BCF024AFF95");

            entity.ToTable("Order");

            entity.Property(e => e.OrderId).ValueGeneratedNever();
            entity.Property(e => e.OrderDate).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_Order_Customer");
        });

        modelBuilder.Entity<OrderItemEntity>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("OrderItem");

            entity.HasOne(d => d.Order).WithMany()
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderItem_Order");

            entity.HasOne(d => d.SkuNavigation).WithMany()
                .HasForeignKey(d => d.Sku)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderItem_Item");
        });

        modelBuilder.Entity<PackageEntity>(entity =>
        {
            entity.HasKey(e => e.PackageId).HasName("PK__Package__322035CCB13E1AE6");

            entity.ToTable("Package");

            entity.Property(e => e.PackageId).ValueGeneratedNever();
            entity.Property(e => e.PackageName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PackageItemEntity>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PackageItem");

            entity.HasOne(d => d.Package).WithMany()
                .HasForeignKey(d => d.PackageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PackageItem_Package");

            entity.HasOne(d => d.SkuNavigation).WithMany()
                .HasForeignKey(d => d.Sku)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PackageItem_Item");
        });

        modelBuilder.Entity<PermissionEntity>(entity =>
        {
            entity.HasKey(e => e.PermissionId).HasName("PK__Permissi__EFA6FB2FC59A3517");

            entity.ToTable("Permission");

            entity.Property(e => e.PermissionId).ValueGeneratedNever();
            entity.Property(e => e.PermissionDescription).HasColumnType("text");
            entity.Property(e => e.PermissionName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<RoleEntity>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Role__8AFACE1A2011D1B1");

            entity.ToTable("Role");

            entity.Property(e => e.RoleId).ValueGeneratedNever();
            entity.Property(e => e.RoleDescription).HasColumnType("text");
            entity.Property(e => e.RoleName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<RolePermissionEntity>(entity =>
        {
            entity.HasKey(e => e.RoleId);

            entity.ToTable("RolePermission");

            entity.Property(e => e.RoleId).ValueGeneratedNever();

            entity.HasOne(d => d.Permission).WithMany(p => p.RolePermissions)
                .HasForeignKey(d => d.PermissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RolePermission_Permission");
        });

        modelBuilder.Entity<SupplierEntity>(entity =>
        {
            entity.HasKey(e => e.SupplierId).HasName("PK__Supplier__4BE666B4DF7753F2");

            entity.ToTable("Supplier");

            entity.Property(e => e.SupplierId).ValueGeneratedNever();
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.SupplierAddress).HasColumnType("text");
        });

        modelBuilder.Entity<SupplierItemEntity>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("SupplierItem");

            entity.HasOne(d => d.SkuNavigation).WithMany()
                .HasForeignKey(d => d.Sku)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SupplierItem_Item");

            entity.HasOne(d => d.Supplier).WithMany()
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SupplierItem_Supplier");
        });

        modelBuilder.Entity<UserEntity>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4CC6DAD6B7");

            entity.HasIndex(e => e.UserEmail, "UQ__Users__08638DF888EF9CA9").IsUnique();

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E41539B69F").IsUnique();

            entity.ToTable("User");

            entity.Property(e => e.UserId).ValueGeneratedNever();
            entity.Property(e => e.UserEmail)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UserFirstName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UserLastName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UserPassword)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Branch).WithMany(p => p.Users)
                .HasForeignKey(d => d.BranchId)
                .HasConstraintName("FK_Users_Branch");
        });

        modelBuilder.Entity<UserEntity>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4CC6DAD6B7");

            entity.HasIndex(e => e.UserEmail, "UQ__Users__08638DF888EF9CA9").IsUnique();

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E41539B69F").IsUnique();

            entity.Property(e => e.UserId).ValueGeneratedNever();
            entity.Property(e => e.UserEmail)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UserFirstName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UserLastName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UserPassword)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Branch).WithMany(p => p.Users)
                .HasForeignKey(d => d.BranchId)
                .HasConstraintName("FK_Users_Branch");
        });
        modelBuilder.Entity<UserRoleEntity>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.ToTable("UserRole");

            entity.Property(e => e.UserId).ValueGeneratedNever();

            entity.HasOne(d => d.Role).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserRole_Role");

            entity.HasOne(d => d.RoleNavigation).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserRole_RolePermission");

            entity.HasOne(d => d.User).WithOne(p => p.UserRole)
                .HasForeignKey<UserRoleEntity>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserRole_Users");
        });

        modelBuilder.Entity<WarehouseEntity>(entity =>
        {
            entity.HasKey(e => e.WarehouseId).HasName("PK__Warehous__2608AFF9068F2E75");

            entity.ToTable("Warehouse");

            entity.Property(e => e.WarehouseId).ValueGeneratedNever();
            entity.Property(e => e.Location)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Branch).WithMany(p => p.Warehouses)
                .HasForeignKey(d => d.BranchId)
                .HasConstraintName("FK_Warehouse_Branch");
        });

        modelBuilder.Entity<WarehouseItemEntity>(entity =>
        {
            entity.HasKey(wi => new { wi.WarehouseId, wi.Sku });
            entity.Property(e => e.Sku).HasColumnName("Sku");  // Explicit column mapping
            entity.Property(e => e.WarehouseId).HasColumnName("WarehouseId");  // Explicit column mapping


            // Define relationship with Item (SkuNavigation)
            entity.HasOne(d => d.SkuNavigation)
                .WithMany()
                .HasForeignKey(d => d.Sku)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WarehouseItem_Item");

            // Define relationship with Warehouse
            entity.HasOne(d => d.Warehouse)
                .WithMany()
                .HasForeignKey(d => d.WarehouseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WarehouseItem_Warehouse");

            entity.ToTable("WarehouseItem");  // Make sure the table name is correct
        });


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
