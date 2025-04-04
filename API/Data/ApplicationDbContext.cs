using System;
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
            entity.HasKey(e => e.BranchId).HasName("PK__BranchEn__A1682FC55618BAEF");

            entity.ToTable("BranchEntity");

            entity.Property(e => e.BranchId).ValueGeneratedOnAdd();

            entity.Property(e => e.BranchName)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.Property(e => e.BranchCity)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.Property(e => e.BranchAddress)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.Property(e => e.BranchRegion)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.Property(e => e.BranchContactNumber)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.Property(e => e.BranchContactEmail)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasMany(e => e.UsersEntities)
                .WithOne(u => u.Branch)
                .HasForeignKey(u => u.BranchId)
                .HasConstraintName("FK_User_Branch")
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasMany(e => e.WarehouseEntities)
                .WithOne(w => w.Branch)
                .HasForeignKey(w => w.BranchId)
                .HasConstraintName("FK_Warehouse_Branch")
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<CartEntity>(entity =>
        {
            entity.HasKey(e => e.CartId).HasName("PK__CartEnti__51BCD7B7270C1AF1");

            entity.ToTable("CartEntity");

            entity.Property(e => e.CartId).ValueGeneratedOnAdd();

            entity.Property(e => e.CustomerId).IsRequired();

            entity.HasOne(d => d.Customer)
                .WithMany(p => p.CartEntities)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cart_Customer");
        });

        modelBuilder.Entity<CartItemEntity>(entity =>
        {
            entity.HasKey(e => new { e.CartId, e.Sku }).HasName("PK_CartItem");
            entity.ToTable("CartItemEntity");

            entity.Property(e => e.CartId).IsRequired();
            entity.Property(e => e.Sku).IsRequired();
            entity.Property(e => e.ItemQuantity).IsRequired();

            entity.HasOne(d => d.Cart)
                .WithMany()
                .HasForeignKey(d => d.CartId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CartItem_Cart");

            entity.HasOne(d => d.Item)
                .WithMany()
                .HasForeignKey(d => d.Sku)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CartItem_Item");
        });

        modelBuilder.Entity<CatalogCategoryEntity>(entity =>
        {
            entity.HasKey(e => new { e.CatalogId, e.CategoryId }).HasName("PK_CatalogCategory");
            entity.ToTable("CatalogCategoryEntity");

            entity.Property(e => e.CategoryName)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Catalog)
                .WithMany(p => p.CatalogCategoryEntities)
                .HasForeignKey(d => d.CatalogId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CatalogCategoryEntity_CatalogEntity");

            entity.HasOne(d => d.Category)
                .WithMany(p => p.CatalogCategoryEntities)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CatalogCategoryEntity_CategoryEntity");
        });

        modelBuilder.Entity<CatalogEntity>(entity =>
        {
            entity.HasKey(e => e.CatalogId).HasName("PK__CatalogE__C2513B68E5CA5841");

            entity.ToTable("CatalogEntity");

            entity.Property(e => e.CatalogId).ValueGeneratedOnAdd();

            entity.Property(e => e.CatalogName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .IsRequired();

            entity.Property(e => e.CatalogDescription)
                .IsUnicode(false)
                .IsRequired();

            entity.Property(e => e.CatalogStatus)
                .IsRequired();
        });

        modelBuilder.Entity<CategoryEntity>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__19093A0B621C3C0F");

            entity.ToTable("CategoryEntity");

            entity.Property(e => e.CategoryId).ValueGeneratedNever();
            entity.Property(e => e.CategoryName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CustomerEntity>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__A4AE64D808AE7D2E");

            entity.ToTable("CustomerEntity");

            entity.Property(e => e.CustomerId).ValueGeneratedOnAdd();

            entity.Property(e => e.CustomerType)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.CustomerName)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.Property(e => e.CustomerContactNumber)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.Property(e => e.CustomerContactEmail)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ItemEntity>(entity =>
        {
            entity.HasKey(e => e.Sku).HasName("PK__ItemEnti__CA1FD3C42F1DE13F");

            entity.ToTable("ItemEntity");

            entity.Property(e => e.Sku).ValueGeneratedOnAdd();

            entity.Property(e => e.ItemName)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.Property(e => e.ItemDescription)
                .IsUnicode(false);

            entity.Property(e => e.ItemCurrency)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.Property(e => e.ItemUnitCost).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ItemMarginGain).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ItemDiscount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ItemAdditionalTax).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ItemPrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ItemImage).IsUnicode(false);

            entity.HasOne(d => d.Category)
                .WithMany(p => p.ItemEntities)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Item_Category");
        });

        modelBuilder.Entity<LineOfCreditEntity>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__LineOfCr__A4AE64D8A717352D");

            entity.ToTable("LineOfCreditEntity");

            entity.Property(e => e.CustomerId).ValueGeneratedNever();
            entity.Property(e => e.CreditLimit).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.CurrentBalance).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Customer)
                .WithOne(p => p.LineOfCreditEntity)
                .HasForeignKey<LineOfCreditEntity>(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LineOfCredit_Customer");
        });

        modelBuilder.Entity<OrderEntity>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__OrderEnt__C3905BCFFF9C8194");

            entity.ToTable("OrderEntity");

            entity.Property(e => e.OrderId).ValueGeneratedOnAdd();

            entity.Property(e => e.OrderDate).HasColumnType("datetime");

            entity.Property(e => e.OrderStatus)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.OrderTotalAmount)
                .HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Customer)
                .WithMany(p => p.OrderEntities)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Customer");
        });

        modelBuilder.Entity<OrderItemEntity>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.Sku }).HasName("PK_OrderItem");
            entity.ToTable("OrderItemEntity");

            entity.Property(e => e.OrderId).IsRequired();
            entity.Property(e => e.Sku).IsRequired();
            entity.Property(e => e.ItemQuantity).IsRequired();

            entity.HasOne(d => d.Order)
                .WithMany(p => p.OrderItemEntities)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderItem_Order");

            entity.HasOne(d => d.Item)
                .WithMany(p => p.OrderItemEntities)
                .HasForeignKey(d => d.Sku)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderItem_Item");
        });

        modelBuilder.Entity<PackageEntity>(entity =>
        {
            entity.HasKey(e => e.PackageId).HasName("PK__PackageE__322035CC4549FE46");

            entity.ToTable("PackageEntity");

            entity.Property(e => e.PackageId).ValueGeneratedOnAdd();

            entity.Property(e => e.PackageName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PackageItemEntity>(entity =>
        {
            entity.HasKey(e => new { e.PackageId, e.Sku }).HasName("PK_PackageItem");

            entity.ToTable("PackageItemEntity");

            entity.Property(e => e.PackageId).IsRequired();
            entity.Property(e => e.Sku).IsRequired();
            entity.Property(e => e.ItemQuantity).IsRequired();

            entity.HasOne(d => d.Package)
                .WithMany(p => p.PackageItemEntities)
                .HasForeignKey(d => d.PackageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PackageItem_Package");

            entity.HasOne(d => d.Item)
                .WithMany(p => p.PackageItemEntities)
                .HasForeignKey(d => d.Sku)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PackageItem_Item");
        });

        modelBuilder.Entity<PermissionEntity>(entity =>
        {
            entity.HasKey(e => e.PermissionId).HasName("PK__Permissi__EFA6FB2FB8BDB8EC");

            entity.ToTable("PermissionEntity");

            entity.Property(e => e.PermissionDescription).IsUnicode(false);
            entity.Property(e => e.PermissionName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<RoleEntity>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__RoleEnti__8AFACE1ADD010900");

            entity.ToTable("RoleEntity");

            entity.Property(e => e.RoleDescription).IsUnicode(false);
            entity.Property(e => e.RoleName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<RolePermissionEntity>(entity =>
        {
            entity.HasKey(e => new { e.RoleId, e.PermissionId }).HasName("PK_RolePermission");

            entity.ToTable("RolePermissionEntity");

            entity.Property(e => e.RoleName)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.Property(e => e.PermissionName)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Role)
                .WithMany(p => p.RolePermissionEntities)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RolePermission_Role");

            entity.HasOne(d => d.Permission)
                .WithMany(p => p.RolePermissionEntities)
                .HasForeignKey(d => d.PermissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RolePermission_Permission");
        });

        modelBuilder.Entity<SupplierEntity>(entity =>
        {
            entity.HasKey(e => e.SupplierId).HasName("PK__Supplier__4BE666B4CEF6C286");

            entity.ToTable("SupplierEntity");

            entity.Property(e => e.SupplierId).ValueGeneratedOnAdd();

            entity.Property(e => e.SupplierName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .IsRequired();

            entity.Property(e => e.SupplierAddress)
                .IsUnicode(false)
                .IsRequired();

            entity.Property(e => e.SupplierContactNumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsRequired();

            entity.Property(e => e.SupplierContactEmail)
                .HasMaxLength(255)
                .IsUnicode(false)
                .IsRequired();
        });

        modelBuilder.Entity<SupplierItemEntity>(entity =>
        {
            entity.HasKey(e => new { e.SupplierId, e.Sku }).HasName("PK_SupplierItem");

            entity.ToTable("SupplierItemEntity");

            entity.Property(e => e.ItemQuantity).IsRequired();

            entity.HasOne(d => d.Supplier)
                .WithMany(p => p.SupplierItemEntities)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SupplierItem_Supplier");

            entity.HasOne(d => d.Item)
                .WithMany(p => p.SupplierItemEntities)
                .HasForeignKey(d => d.Sku)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SupplierItem_Item");
        });

        modelBuilder.Entity<UserRoleEntity>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.RoleId }).HasName("PK_UserRole");

            entity.ToTable("UserRoleEntity");

            entity.Property(e => e.RoleName)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.User)
                .WithMany(p => p.UserRoleEntities)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserRole_User");

            entity.HasOne(d => d.Role)
                .WithMany(p => p.UserRoleEntities)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserRole_Role");
        });

        modelBuilder.Entity<UserEntity>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__UsersEnt__1788CC4CAA75799E");

            entity.ToTable("UserEntity");

            entity.HasIndex(e => e.UserContactEmail)
                .IsUnique()
                .HasDatabaseName("UQ__UserEnt__08638DF886582A61");

            entity.Property(e => e.UserId).ValueGeneratedOnAdd();

            entity.Property(e => e.UserFirstName)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.Property(e => e.UserLastName)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.Property(e => e.UserContactEmail)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.Property(e => e.UserContactNumber)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.Property(e => e.UserPassword)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Branch)
                .WithMany(p => p.UsersEntities)
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Branch");
        });

        modelBuilder.Entity<WarehouseEntity>(entity =>
        {
            entity.HasKey(e => e.WarehouseId).HasName("PK__Warehous__2608AFF92853480B");

            entity.ToTable("WarehouseEntity");

            entity.Property(e => e.WarehouseId).ValueGeneratedOnAdd();

            entity.Property(e => e.WarehouseLocation)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.Property(e => e.WarehouseCapacity)
                .IsRequired();

            entity.Property(e => e.BranchId)
                .IsRequired();

            entity.HasOne(d => d.Branch)
                .WithMany(p => p.WarehouseEntities)
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Warehouse_Branch");
        });

        modelBuilder.Entity<WarehouseItemEntity>(entity =>
        {
            entity.HasKey(e => new { e.WarehouseId, e.Sku }).HasName("PK_WarehouseItem");
            entity.ToTable("WarehouseItemEntity");

            entity.Property(e => e.WarehouseId).IsRequired();
            entity.Property(e => e.Sku).IsRequired();
            entity.Property(e => e.ItemQuantity).IsRequired();

            entity.HasOne(d => d.Warehouse)
                .WithMany(p => p.WarehouseItemEntities)
                .HasForeignKey(d => d.WarehouseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WarehouseItem_Warehouse");

            entity.HasOne(d => d.Item)
                .WithMany(p => p.WarehouseItemEntities)
                .HasForeignKey(d => d.Sku)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WarehouseItem_Item");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
