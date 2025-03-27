﻿// <auto-generated />
using System;
using API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace API.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250327221543_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("API.Entities.Branch", b =>
                {
                    b.Property<int>("BranchId")
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<string>("City")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ContactEmail")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ContactNumber")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Name")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Region")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.HasKey("BranchId")
                        .HasName("PK__Branch__A1682FC500954FE5");

                    b.ToTable("Branch", (string)null);
                });

            modelBuilder.Entity("API.Entities.Cart", b =>
                {
                    b.Property<int>("CartId")
                        .HasColumnType("int");

                    b.Property<int?>("CustomerId")
                        .HasColumnType("int");

                    b.HasKey("CartId")
                        .HasName("PK__Cart__51BCD7B7103B1C89");

                    b.HasIndex("CustomerId");

                    b.ToTable("Cart", (string)null);
                });

            modelBuilder.Entity("API.Entities.CartItem", b =>
                {
                    b.Property<int>("CartId")
                        .HasColumnType("int");

                    b.Property<int>("Sku")
                        .HasColumnType("int");

                    b.HasIndex("CartId");

                    b.HasIndex("Sku");

                    b.ToTable("CartItem", (string)null);
                });

            modelBuilder.Entity("API.Entities.Catalog", b =>
                {
                    b.Property<int>("CatalogId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<bool?>("Status")
                        .HasColumnType("bit");

                    b.HasKey("CatalogId")
                        .HasName("PK__Catalog__C2513B6835A0C5A2");

                    b.ToTable("Catalog", (string)null);
                });

            modelBuilder.Entity("API.Entities.CatalogCategory", b =>
                {
                    b.Property<int>("CatalogId")
                        .HasColumnType("int");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.HasIndex("CatalogId");

                    b.HasIndex("CategoryId");

                    b.ToTable("CatalogCategory", (string)null);
                });

            modelBuilder.Entity("API.Entities.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<bool?>("Status")
                        .HasColumnType("bit");

                    b.HasKey("CategoryId")
                        .HasName("PK__Category__19093A0B813139CC");

                    b.ToTable("Category", (string)null);
                });

            modelBuilder.Entity("API.Entities.CategoryItem", b =>
                {
                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<int>("Sku")
                        .HasColumnType("int");

                    b.HasIndex("CategoryId");

                    b.HasIndex("Sku");

                    b.ToTable("CategoryItem", (string)null);
                });

            modelBuilder.Entity("API.Entities.Customer", b =>
                {
                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<string>("CustomerType")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Email")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Name")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.HasKey("CustomerId")
                        .HasName("PK__Customer__A4AE64D892601F68");

                    b.ToTable("Customer", (string)null);
                });

            modelBuilder.Entity("API.Entities.Item", b =>
                {
                    b.Property<int>("Sku")
                        .HasColumnType("int");

                    b.Property<decimal?>("AdditionalTax")
                        .HasColumnType("decimal(10, 2)");

                    b.Property<int?>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Currency")
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("varchar(10)");

                    b.Property<int?>("CurrentStock")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<decimal?>("Discount")
                        .HasColumnType("decimal(10, 2)");

                    b.Property<string>("Image")
                        .HasColumnType("text");

                    b.Property<decimal?>("ItemPrice")
                        .HasColumnType("decimal(10, 2)");

                    b.Property<bool?>("ItemStatus")
                        .HasColumnType("bit");

                    b.Property<decimal?>("MarginGain")
                        .HasColumnType("decimal(10, 2)");

                    b.Property<string>("Name")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<int?>("OriginalStock")
                        .HasColumnType("int");

                    b.Property<decimal?>("UnitCost")
                        .HasColumnType("decimal(10, 2)");

                    b.HasKey("Sku")
                        .HasName("PK__Item__CA1FD3C4297D061E");

                    b.HasIndex("CategoryId");

                    b.ToTable("Item", (string)null);
                });

            modelBuilder.Entity("API.Entities.LineOfCredit", b =>
                {
                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<decimal?>("CreditLimit")
                        .HasColumnType("decimal(10, 2)");

                    b.Property<decimal?>("CurrentBalance")
                        .HasColumnType("decimal(10, 2)");

                    b.HasKey("CustomerId")
                        .HasName("PK__LineOfCr__A4AE64D8236E36B3");

                    b.ToTable("LineOfCredit", (string)null);
                });

            modelBuilder.Entity("API.Entities.Order", b =>
                {
                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<int?>("CustomerId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("OrderDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Status")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<decimal?>("TotalAmount")
                        .HasColumnType("decimal(10, 2)");

                    b.HasKey("OrderId")
                        .HasName("PK__Order__C3905BCF024AFF95");

                    b.HasIndex("CustomerId");

                    b.ToTable("Order", (string)null);
                });

            modelBuilder.Entity("API.Entities.OrderItem", b =>
                {
                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<int?>("Quantity")
                        .HasColumnType("int");

                    b.Property<int>("Sku")
                        .HasColumnType("int");

                    b.HasIndex("OrderId");

                    b.HasIndex("Sku");

                    b.ToTable("OrderItem", (string)null);
                });

            modelBuilder.Entity("API.Entities.Package", b =>
                {
                    b.Property<int>("PackageId")
                        .HasColumnType("int");

                    b.Property<string>("PackageName")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.HasKey("PackageId")
                        .HasName("PK__Package__322035CCB13E1AE6");

                    b.ToTable("Package", (string)null);
                });

            modelBuilder.Entity("API.Entities.PackageItem", b =>
                {
                    b.Property<int>("PackageId")
                        .HasColumnType("int");

                    b.Property<int>("Sku")
                        .HasColumnType("int");

                    b.HasIndex("PackageId");

                    b.HasIndex("Sku");

                    b.ToTable("PackageItem", (string)null);
                });

            modelBuilder.Entity("API.Entities.Permission", b =>
                {
                    b.Property<int>("PermissionId")
                        .HasColumnType("int");

                    b.Property<string>("PermissionDescription")
                        .HasColumnType("text");

                    b.Property<string>("PermissionName")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.HasKey("PermissionId")
                        .HasName("PK__Permissi__EFA6FB2FC59A3517");

                    b.ToTable("Permission", (string)null);
                });

            modelBuilder.Entity("API.Entities.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<string>("RoleDescription")
                        .HasColumnType("text");

                    b.Property<string>("RoleName")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<bool?>("RoleStatus")
                        .HasColumnType("bit");

                    b.HasKey("RoleId")
                        .HasName("PK__Role__8AFACE1A2011D1B1");

                    b.ToTable("Role", (string)null);
                });

            modelBuilder.Entity("API.Entities.RolePermission", b =>
                {
                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<int>("PermissionId")
                        .HasColumnType("int");

                    b.HasKey("RoleId");

                    b.HasIndex("PermissionId");

                    b.ToTable("RolePermission", (string)null);
                });

            modelBuilder.Entity("API.Entities.Supplier", b =>
                {
                    b.Property<int>("SupplierId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("SupplierAddress")
                        .HasColumnType("text");

                    b.HasKey("SupplierId")
                        .HasName("PK__Supplier__4BE666B4DF7753F2");

                    b.ToTable("Supplier", (string)null);
                });

            modelBuilder.Entity("API.Entities.SupplierItem", b =>
                {
                    b.Property<int>("Sku")
                        .HasColumnType("int");

                    b.Property<int>("SupplierId")
                        .HasColumnType("int");

                    b.HasIndex("Sku");

                    b.HasIndex("SupplierId");

                    b.ToTable("SupplierItem", (string)null);
                });

            modelBuilder.Entity("API.Entities.User", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int?>("BranchId")
                        .HasColumnType("int");

                    b.Property<bool?>("IsBlocked")
                        .HasColumnType("bit");

                    b.Property<string>("UserEmail")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("UserFirstName")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("UserLastName")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("UserPassword")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<bool?>("UserStatus")
                        .HasColumnType("bit");

                    b.Property<string>("Username")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.HasKey("UserId")
                        .HasName("PK__Users__1788CC4CC6DAD6B7");

                    b.HasIndex("BranchId");

                    b.HasIndex(new[] { "UserEmail" }, "UQ__Users__08638DF888EF9CA9")
                        .IsUnique()
                        .HasFilter("[UserEmail] IS NOT NULL");

                    b.HasIndex(new[] { "Username" }, "UQ__Users__536C85E41539B69F")
                        .IsUnique()
                        .HasFilter("[Username] IS NOT NULL");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("API.Entities.UserRole", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("UserId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRole", (string)null);
                });

            modelBuilder.Entity("API.Entities.Warehouse", b =>
                {
                    b.Property<int>("WarehouseId")
                        .HasColumnType("int");

                    b.Property<int?>("BranchId")
                        .HasColumnType("int");

                    b.Property<int?>("Capacity")
                        .HasColumnType("int");

                    b.Property<string>("Location")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.HasKey("WarehouseId")
                        .HasName("PK__Warehous__2608AFF9068F2E75");

                    b.HasIndex("BranchId");

                    b.ToTable("Warehouse", (string)null);
                });

            modelBuilder.Entity("API.Entities.WarehouseItem", b =>
                {
                    b.Property<int>("Sku")
                        .HasColumnType("int");

                    b.Property<int?>("Stock")
                        .HasColumnType("int");

                    b.Property<int>("WarehouseId")
                        .HasColumnType("int");

                    b.HasIndex("Sku");

                    b.HasIndex("WarehouseId");

                    b.ToTable("WarehouseItem", (string)null);
                });

            modelBuilder.Entity("API.Entities.Cart", b =>
                {
                    b.HasOne("API.Entities.Customer", "Customer")
                        .WithMany("Carts")
                        .HasForeignKey("CustomerId")
                        .HasConstraintName("FK_Cart_Customer");

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("API.Entities.CartItem", b =>
                {
                    b.HasOne("API.Entities.Cart", "Cart")
                        .WithMany()
                        .HasForeignKey("CartId")
                        .IsRequired()
                        .HasConstraintName("FK_CartItem_Cart");

                    b.HasOne("API.Entities.Item", "SkuNavigation")
                        .WithMany()
                        .HasForeignKey("Sku")
                        .IsRequired()
                        .HasConstraintName("FK_CartItem_Item");

                    b.Navigation("Cart");

                    b.Navigation("SkuNavigation");
                });

            modelBuilder.Entity("API.Entities.CatalogCategory", b =>
                {
                    b.HasOne("API.Entities.Catalog", "Catalog")
                        .WithMany()
                        .HasForeignKey("CatalogId")
                        .IsRequired()
                        .HasConstraintName("FK_CatalogCategory_Catalog");

                    b.HasOne("API.Entities.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .IsRequired()
                        .HasConstraintName("FK_CatalogCategory_Category");

                    b.Navigation("Catalog");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("API.Entities.CategoryItem", b =>
                {
                    b.HasOne("API.Entities.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .IsRequired()
                        .HasConstraintName("FK_CategoryItem_Category");

                    b.HasOne("API.Entities.Item", "SkuNavigation")
                        .WithMany()
                        .HasForeignKey("Sku")
                        .IsRequired()
                        .HasConstraintName("FK_CategoryItem_Item");

                    b.Navigation("Category");

                    b.Navigation("SkuNavigation");
                });

            modelBuilder.Entity("API.Entities.Item", b =>
                {
                    b.HasOne("API.Entities.Category", "Category")
                        .WithMany("Items")
                        .HasForeignKey("CategoryId")
                        .HasConstraintName("FK_Item_Category");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("API.Entities.LineOfCredit", b =>
                {
                    b.HasOne("API.Entities.Customer", "Customer")
                        .WithOne("LineOfCredit")
                        .HasForeignKey("API.Entities.LineOfCredit", "CustomerId")
                        .IsRequired()
                        .HasConstraintName("FK_LineOfCredit_Customer");

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("API.Entities.Order", b =>
                {
                    b.HasOne("API.Entities.Customer", "Customer")
                        .WithMany("Orders")
                        .HasForeignKey("CustomerId")
                        .HasConstraintName("FK_Order_Customer");

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("API.Entities.OrderItem", b =>
                {
                    b.HasOne("API.Entities.Order", "Order")
                        .WithMany()
                        .HasForeignKey("OrderId")
                        .IsRequired()
                        .HasConstraintName("FK_OrderItem_Order");

                    b.HasOne("API.Entities.Item", "SkuNavigation")
                        .WithMany()
                        .HasForeignKey("Sku")
                        .IsRequired()
                        .HasConstraintName("FK_OrderItem_Item");

                    b.Navigation("Order");

                    b.Navigation("SkuNavigation");
                });

            modelBuilder.Entity("API.Entities.PackageItem", b =>
                {
                    b.HasOne("API.Entities.Package", "Package")
                        .WithMany()
                        .HasForeignKey("PackageId")
                        .IsRequired()
                        .HasConstraintName("FK_PackageItem_Package");

                    b.HasOne("API.Entities.Item", "SkuNavigation")
                        .WithMany()
                        .HasForeignKey("Sku")
                        .IsRequired()
                        .HasConstraintName("FK_PackageItem_Item");

                    b.Navigation("Package");

                    b.Navigation("SkuNavigation");
                });

            modelBuilder.Entity("API.Entities.RolePermission", b =>
                {
                    b.HasOne("API.Entities.Permission", "Permission")
                        .WithMany("RolePermissions")
                        .HasForeignKey("PermissionId")
                        .IsRequired()
                        .HasConstraintName("FK_RolePermission_Permission");

                    b.Navigation("Permission");
                });

            modelBuilder.Entity("API.Entities.SupplierItem", b =>
                {
                    b.HasOne("API.Entities.Item", "SkuNavigation")
                        .WithMany()
                        .HasForeignKey("Sku")
                        .IsRequired()
                        .HasConstraintName("FK_SupplierItem_Item");

                    b.HasOne("API.Entities.Supplier", "Supplier")
                        .WithMany()
                        .HasForeignKey("SupplierId")
                        .IsRequired()
                        .HasConstraintName("FK_SupplierItem_Supplier");

                    b.Navigation("SkuNavigation");

                    b.Navigation("Supplier");
                });

            modelBuilder.Entity("API.Entities.User", b =>
                {
                    b.HasOne("API.Entities.Branch", "Branch")
                        .WithMany("Users")
                        .HasForeignKey("BranchId")
                        .HasConstraintName("FK_Users_Branch");

                    b.Navigation("Branch");
                });

            modelBuilder.Entity("API.Entities.UserRole", b =>
                {
                    b.HasOne("API.Entities.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .IsRequired()
                        .HasConstraintName("FK_UserRole_Role");

                    b.HasOne("API.Entities.RolePermission", "RoleNavigation")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .IsRequired()
                        .HasConstraintName("FK_UserRole_RolePermission");

                    b.HasOne("API.Entities.User", "User")
                        .WithOne("UserRole")
                        .HasForeignKey("API.Entities.UserRole", "UserId")
                        .IsRequired()
                        .HasConstraintName("FK_UserRole_Users");

                    b.Navigation("Role");

                    b.Navigation("RoleNavigation");

                    b.Navigation("User");
                });

            modelBuilder.Entity("API.Entities.Warehouse", b =>
                {
                    b.HasOne("API.Entities.Branch", "Branch")
                        .WithMany("Warehouses")
                        .HasForeignKey("BranchId")
                        .HasConstraintName("FK_Warehouse_Branch");

                    b.Navigation("Branch");
                });

            modelBuilder.Entity("API.Entities.WarehouseItem", b =>
                {
                    b.HasOne("API.Entities.Item", "SkuNavigation")
                        .WithMany()
                        .HasForeignKey("Sku")
                        .IsRequired()
                        .HasConstraintName("FK_WarehouseItem_Item");

                    b.HasOne("API.Entities.Warehouse", "Warehouse")
                        .WithMany()
                        .HasForeignKey("WarehouseId")
                        .IsRequired()
                        .HasConstraintName("FK_WarehouseItem_Warehouse");

                    b.Navigation("SkuNavigation");

                    b.Navigation("Warehouse");
                });

            modelBuilder.Entity("API.Entities.Branch", b =>
                {
                    b.Navigation("Users");

                    b.Navigation("Warehouses");
                });

            modelBuilder.Entity("API.Entities.Category", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("API.Entities.Customer", b =>
                {
                    b.Navigation("Carts");

                    b.Navigation("LineOfCredit");

                    b.Navigation("Orders");
                });

            modelBuilder.Entity("API.Entities.Permission", b =>
                {
                    b.Navigation("RolePermissions");
                });

            modelBuilder.Entity("API.Entities.Role", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("API.Entities.RolePermission", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("API.Entities.User", b =>
                {
                    b.Navigation("UserRole");
                });
#pragma warning restore 612, 618
        }
    }
}
