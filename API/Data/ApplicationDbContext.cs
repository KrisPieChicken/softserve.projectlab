using System;
using System.Collections.Generic;
using API;
using Microsoft.EntityFrameworkCore;
using API.Data.Entities;

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

    public virtual DbSet<BranchEntity> BranchEntities { get; set; }

    public virtual DbSet<CartEntity> CartEntities { get; set; }

    public virtual DbSet<CartItemEntity> CartItemEntities { get; set; }

    public virtual DbSet<CatalogCategoryEntity> CatalogCategoryEntities { get; set; }

    public virtual DbSet<CatalogEntity> CatalogEntities { get; set; }

    public virtual DbSet<CategoryEntity> CategoryEntities { get; set; }

    public virtual DbSet<CustomerEntity> CustomerEntities { get; set; }

    public virtual DbSet<ItemEntity> ItemEntities { get; set; }

    public virtual DbSet<OrderEntity> OrderEntities { get; set; }

    public virtual DbSet<OrderItemEntity> OrderItemEntities { get; set; }

    public virtual DbSet<PackageEntity> PackageEntities { get; set; }

    public virtual DbSet<PackageItemEntity> PackageItemEntities { get; set; }

    public virtual DbSet<PackageNoteEntity> PackageNoteEntities { get; set; }

    public virtual DbSet<PermissionEntity> PermissionEntities { get; set; }

    public virtual DbSet<RoleEntity> RoleEntities { get; set; }

    public virtual DbSet<RolePermissionEntity> RolePermissionEntities { get; set; }

    public virtual DbSet<SupplierEntity> SupplierEntities { get; set; }

    public virtual DbSet<SupplierItemEntity> SupplierItemEntities { get; set; }

    public virtual DbSet<UserEntity> UserEntities { get; set; }

    public virtual DbSet<UserRoleEntity> UserRoleEntities { get; set; }

    public virtual DbSet<WarehouseEntity> WarehouseEntities { get; set; }

    public virtual DbSet<WarehouseItemEntity> WarehouseItemEntities { get; set; }

    /*
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-GH8PPU1;Database=RanAwayDB;Integrated Security=True;TrustServerCertificate=True;");
    */
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BranchEntity>(entity =>
        {
            entity.HasKey(e => e.BranchId).HasName("PK__BranchEn__A1682FC5CCCD0D1D");

            entity.ToTable("BranchEntity");

            entity.Property(e => e.BranchAddress)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.BranchCity)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.BranchContactEmail)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.BranchContactNumber)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.BranchName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.BranchRegion)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<CartEntity>(entity =>
        {
            entity.HasKey(e => e.CartId).HasName("PK__CartEnti__51BCD7B7485CD17F");

            entity.ToTable("CartEntity");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Customer).WithMany(p => p.CartEntities)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cart_Customer");
        });

        modelBuilder.Entity<CartItemEntity>(entity =>
        {
            entity.HasKey(e => new { e.CartId, e.Sku }).HasName("PK__CartItem__0D1D2A8B300FE4DB");

            entity.ToTable("CartItemEntity");

            entity.HasOne(d => d.Cart).WithMany(p => p.CartItemEntities)
                .HasForeignKey(d => d.CartId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CartItem_Cart");

            entity.HasOne(d => d.SkuNavigation).WithMany(p => p.CartItemEntities)
                .HasForeignKey(d => d.Sku)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CartItem_Item");
        });

        modelBuilder.Entity<CatalogCategoryEntity>(entity =>
        {
            entity.HasKey(e => new { e.CatalogId, e.CategoryId }).HasName("PK__CatalogC__63C1A8C803D2B97B");

            entity.ToTable("CatalogCategoryEntity");

            entity.Property(e => e.CategoryName)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Catalog).WithMany(p => p.CatalogCategoryEntities)
                .HasForeignKey(d => d.CatalogId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CatalogCategoryEntity_CatalogEntity");

            entity.HasOne(d => d.Category).WithMany(p => p.CatalogCategoryEntities)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CatalogCategoryEntity_CategoryEntity");
        });

        modelBuilder.Entity<CatalogEntity>(entity =>
        {
            entity.HasKey(e => e.CatalogId).HasName("PK__CatalogE__C2513B68358ACFA2");

            entity.ToTable("CatalogEntity");

            entity.Property(e => e.CatalogDescription).IsUnicode(false);
            entity.Property(e => e.CatalogName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<CategoryEntity>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__19093A0B0D717C89");

            entity.ToTable("CategoryEntity");

            entity.Property(e => e.CategoryName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<CustomerEntity>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__A4AE64D89D674FAC");

            entity.ToTable("CustomerEntity");

            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CustomerContactEmail)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.CustomerContactNumber)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.CustomerFirstName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CustomerLastName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.State)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ZipCode)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ItemEntity>(entity =>
        {
            entity.HasKey(e => e.Sku).HasName("PK__ItemEnti__CA1FD3C484EE8725");

            entity.ToTable("ItemEntity");

            entity.Property(e => e.Sku).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ItemAdditionalTax).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ItemCurrency)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.ItemDescription).IsUnicode(false);
            entity.Property(e => e.ItemDiscount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ItemId).ValueGeneratedOnAdd();
            entity.Property(e => e.ItemImage).IsUnicode(false);
            entity.Property(e => e.ItemMarginGain).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ItemName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ItemPrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ItemUnitCost).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Category).WithMany(p => p.ItemEntities)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Item_Category");
        });

        modelBuilder.Entity<OrderEntity>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__OrderEnt__C3905BCF485EB924");

            entity.ToTable("OrderEntity");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.OrderDate).HasColumnType("datetime");
            entity.Property(e => e.OrderStatus)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.OrderTotalAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Customer).WithMany(p => p.OrderEntities)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Customer");
        });

        modelBuilder.Entity<OrderItemEntity>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.Sku }).HasName("PK__OrderIte__9F31A6F3EA8B0110");

            entity.ToTable("OrderItemEntity");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItemEntities)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderItem_Order");

            entity.HasOne(d => d.SkuNavigation).WithMany(p => p.OrderItemEntities)
                .HasForeignKey(d => d.Sku)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderItem_Item");
        });

        modelBuilder.Entity<PackageEntity>(entity =>
        {
            entity.HasKey(e => e.PackageId).HasName("PK__PackageE__322035CC08A50029");

            entity.ToTable("PackageEntity");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PackageName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<PackageItemEntity>(entity =>
        {
            entity.HasKey(e => new { e.PackageId, e.Sku }).HasName("PK__PackageI__6E81C8F08B50B3CD");

            entity.ToTable("PackageItemEntity");

            entity.HasOne(d => d.Package).WithMany(p => p.PackageItemEntities)
                .HasForeignKey(d => d.PackageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PackageItem_Package");

            entity.HasOne(d => d.SkuNavigation).WithMany(p => p.PackageItemEntities)
                .HasForeignKey(d => d.Sku)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PackageItem_Item");
        });

        modelBuilder.Entity<PackageNoteEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PackageN__3214EC07DD892105");

            entity.ToTable("PackageNoteEntity");

            entity.Property(e => e.Id)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasDefaultValueSql("(newid())");
            entity.Property(e => e.Content).IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Package).WithMany(p => p.PackageNoteEntities)
                .HasForeignKey(d => d.PackageId)
                .HasConstraintName("FK_PackageNote_Package");
        });

        modelBuilder.Entity<PermissionEntity>(entity =>
        {
            entity.HasKey(e => e.PermissionId).HasName("PK__Permissi__EFA6FB2FD0BACA84");

            entity.ToTable("PermissionEntity");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PermissionDescription).IsUnicode(false);
            entity.Property(e => e.PermissionName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<RoleEntity>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__RoleEnti__8AFACE1AAACCDD25");

            entity.ToTable("RoleEntity");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.RoleDescription).IsUnicode(false);
            entity.Property(e => e.RoleName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<RolePermissionEntity>(entity =>
        {
            entity.HasKey(e => new { e.RoleId, e.PermissionId }).HasName("PK__RolePerm__6400A1A84691FE5A");

            entity.ToTable("RolePermissionEntity");

            entity.Property(e => e.PermissionName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.RoleName)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Permission).WithMany(p => p.RolePermissionEntities)
                .HasForeignKey(d => d.PermissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RolePermission_Permission");

            entity.HasOne(d => d.Role).WithMany(p => p.RolePermissionEntities)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RolePermission_Role");
        });

        modelBuilder.Entity<SupplierEntity>(entity =>
        {
            entity.HasKey(e => e.SupplierId).HasName("PK__Supplier__4BE666B4B773B05C");

            entity.ToTable("SupplierEntity");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.SupplierAddress).IsUnicode(false);
            entity.Property(e => e.SupplierContactEmail)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.SupplierContactNumber)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.SupplierName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<SupplierItemEntity>(entity =>
        {
            entity.HasKey(e => new { e.SupplierId, e.Sku }).HasName("PK__Supplier__17479B888FA110BF");

            entity.ToTable("SupplierItemEntity");

            entity.Property(e => e.ExpectedDeliveryDate).HasColumnType("datetime");
            entity.Property(e => e.OrderDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.SkuNavigation).WithMany(p => p.SupplierItemEntities)
                .HasForeignKey(d => d.Sku)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SupplierItem_Item");

            entity.HasOne(d => d.Supplier).WithMany(p => p.SupplierItemEntities)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SupplierItem_Supplier");
        });

        modelBuilder.Entity<UserEntity>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__UserEnti__1788CC4CE64704DB");

            entity.ToTable("UserEntity");

            entity.HasIndex(e => e.UserContactEmail, "UQ_UserContactEmail").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PasswordHash).HasDefaultValueSql("(0x)");
            entity.Property(e => e.PasswordSalt).HasDefaultValueSql("(0x)");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UserContactEmail)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UserContactNumber)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UserFirstName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UserLastName)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Branch).WithMany(p => p.UserEntities)
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Branch");
        });

        modelBuilder.Entity<UserRoleEntity>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.RoleId }).HasName("PK__UserRole__AF2760ADC2EA7126");

            entity.ToTable("UserRoleEntity");

            entity.Property(e => e.RoleName)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Role).WithMany(p => p.UserRoleEntities)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserRole_Role");

            entity.HasOne(d => d.User).WithMany(p => p.UserRoleEntities)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserRole_User");
        });

        modelBuilder.Entity<WarehouseEntity>(entity =>
        {
            entity.HasKey(e => e.WarehouseId).HasName("PK__Warehous__2608AFF918D7A0C4");

            entity.ToTable("WarehouseEntity");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.WarehouseLocation)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Branch).WithMany(p => p.WarehouseEntities)
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Warehouse_Branch");
        });

        modelBuilder.Entity<WarehouseItemEntity>(entity =>
        {
            entity.HasKey(e => new { e.WarehouseId, e.Sku }).HasName("PK__Warehous__7AA952C54E329386");

            entity.ToTable("WarehouseItemEntity");

            entity.HasOne(d => d.SkuNavigation).WithMany(p => p.WarehouseItemEntities)
                .HasForeignKey(d => d.Sku)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WarehouseItem_Item");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.WarehouseItemEntities)
                .HasForeignKey(d => d.WarehouseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WarehouseItem_Warehouse");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
