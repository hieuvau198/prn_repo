using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PRN222.Lab2.ProductStore.Repository.Models;

public partial class MyStoreDbContext : DbContext
{
    public MyStoreDbContext()
    {
    }

    public MyStoreDbContext(DbContextOptions<MyStoreDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AccountMember> AccountMembers { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=LAPTOP-9MJD423N\\QUYDUC;Database=MyStoreDB;Uid=sa;Pwd=12345;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccountMember>(entity =>
        {
            entity.HasKey(e => e.MemberId).HasName("PK__AccountM__0CF04B183D81C5AF");

            entity.ToTable("AccountMember");

            entity.HasIndex(e => e.EmailAddress, "UQ__AccountM__49A14740F1AA43C2").IsUnique();

            entity.Property(e => e.EmailAddress).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(200);
            entity.Property(e => e.MemberPassword).HasMaxLength(255);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__19093A0B8E941007");

            entity.ToTable("Category");

            entity.Property(e => e.CategoryName).HasMaxLength(100);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Product__B40CC6CD22134D61");

            entity.ToTable("Product");

            entity.Property(e => e.ProductName).HasMaxLength(200);
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Product__Categor__29572725");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
