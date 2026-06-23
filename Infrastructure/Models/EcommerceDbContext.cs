using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Models;

public partial class EcommerceDbContext : DbContext
{
    public EcommerceDbContext()
    {
    }

    public EcommerceDbContext(DbContextOptions<EcommerceDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Coupon> Coupons { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Phone> Phones { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductReview> ProductReviews { get; set; }

    public virtual DbSet<ProductVariant> ProductVariants { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.AddressId).HasName("PK_AddressID");

            entity.ToTable("Address");

            entity.Property(e => e.AddressId).HasColumnName("AddressID");
            entity.Property(e => e.City).HasMaxLength(50);
            entity.Property(e => e.PostalCode).HasMaxLength(30);
            entity.Property(e => e.StreetAddress).HasMaxLength(50);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Address__UserID__71D1E811");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK_Categorys");

            entity.ToTable("Category");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName).HasMaxLength(100);
        });

        modelBuilder.Entity<Coupon>(entity =>
        {
            entity.HasKey(e => e.CouponsId).HasName("PK_CouponID");

            entity.Property(e => e.CouponsId).HasColumnName("CouponsID");
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.DiscountType).HasMaxLength(20);
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.Value).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Image");

            entity.Property(e => e.ImageId)
                .ValueGeneratedOnAdd()
                .HasColumnName("ImageID");
            entity.Property(e => e.ImageUrl).HasMaxLength(250);
            entity.Property(e => e.ProductId).HasColumnName("ProductID");

            entity.HasOne(d => d.Product).WithMany()
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Image__ProductID__797309D9");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK_OrderID");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.CouponsId).HasColumnName("CouponsID");
            entity.Property(e => e.OrderDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.OrderStatus)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Pending");
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Coupons).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CouponsId)
                .HasConstraintName("FK__Orders__CouponsI__10566F31");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Orders__UserID__0F624AF8");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.OrderItemId).HasName("PK_OrderItemID");

            entity.ToTable("OrderItem");

            entity.Property(e => e.OrderItemId).HasColumnName("OrderItemID");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.VariantId).HasColumnName("VariantID");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__OrderItem__Order__160F4887");

            entity.HasOne(d => d.Variant).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.VariantId)
                .HasConstraintName("FK__OrderItem__Varia__151B244E");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payments__9B556A580084DC3E");

            entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.PaymentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PaymentMethod).HasMaxLength(50);
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(50)
                .HasDefaultValue("Pending");
            entity.Property(e => e.TransactionId)
                .HasMaxLength(100)
                .HasColumnName("TransactionID");

            entity.HasOne(d => d.Order).WithMany(p => p.Payments)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_Payments_Orders");
        });

        modelBuilder.Entity<Phone>(entity =>
        {
            entity.HasKey(e => e.PhoneId).HasName("PK_PhoneID");

            entity.ToTable("Phone");

            entity.Property(e => e.PhoneId).HasColumnName("PhoneID");
            entity.Property(e => e.PhoneNumber).HasMaxLength(15);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Phones)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Phone__UserID__74AE54BC");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK_ProductID");

            entity.ToTable("Product");
            entity.HasQueryFilter(p => !p.IsDeleted);

            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.Note).HasMaxLength(100);
            entity.Property(e => e.ProductName).HasMaxLength(50);

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Product__Categor__778AC167");
        });

        modelBuilder.Entity<ProductReview>(entity =>
        {
            entity.HasKey(e => e.ProductReviewsId).HasName("PK_ProductReviewsID");

            entity.Property(e => e.ProductReviewsId).HasColumnName("ProductReviewsID");
            entity.Property(e => e.Comment).HasMaxLength(1000);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductReviews)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__ProductRe__Produ__03F0984C");

            entity.HasOne(d => d.User).WithMany(p => p.ProductReviews)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__ProductRe__UserI__02FC7413");
        });

        modelBuilder.Entity<ProductVariant>(entity =>
        {
            entity.HasKey(e => e.VariantId).HasName("PK_VariantID");

            entity.Property(e => e.VariantId).HasColumnName("VariantID");
            entity.Property(e => e.Color).HasMaxLength(10);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.Size).HasMaxLength(15);

            entity.HasOne(d => d.Product).WithMany(p => p.ProductVariants)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__ProductVa__Produ__7E37BEF6");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK_Roles");

            entity.ToTable("Role");

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<ShoppingCart>(entity =>
        {
            entity.HasKey(e => e.ShoppingCartId).HasName("PK_ShoppingCartID");

            entity.ToTable("ShoppingCart");

            entity.Property(e => e.ShoppingCartId).HasColumnName("ShoppingCartID");
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.VariantId).HasColumnName("VariantID");

            entity.HasOne(d => d.User).WithMany(p => p.ShoppingCarts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__ShoppingC__UserI__07C12930");

            entity.HasOne(d => d.Variant).WithMany(p => p.ShoppingCarts)
                .HasForeignKey(d => d.VariantId)
                .HasConstraintName("FK__ShoppingC__Varia__08B54D69");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK_UserID");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.FirstName).HasMaxLength(30);
            entity.Property(e => e.LastName).HasMaxLength(30);
            entity.Property(e => e.PasswordHash).IsUnicode(false);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.UserName).HasMaxLength(20);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__Users__RoleID__6EF57B66");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
