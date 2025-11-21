using DE.DataLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DE.DataLayer.Context;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DeManufacturer> DeManufacturers { get; set; }

    public virtual DbSet<DeOrder> DeOrders { get; set; }

    public virtual DbSet<DeOrderHasDeProduct> DeOrderHasDeProducts { get; set; }

    public virtual DbSet<DePickupPoint> DePickupPoints { get; set; }

    public virtual DbSet<DeProduct> DeProducts { get; set; }

    public virtual DbSet<DeRole> DeRoles { get; set; }

    public virtual DbSet<DeSupplier> DeSuppliers { get; set; }

    public virtual DbSet<DeUser> DeUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Data Source=mssql;Initial Catalog=ispp2108;User ID=ispp2108;Password=2108;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DeManufacturer>(entity =>
        {
            entity.ToTable("DeManufacturer");

            entity.Property(e => e.Manufacturer).HasMaxLength(150);
        });

        modelBuilder.Entity<DeOrder>(entity =>
        {
            entity.ToTable("DeOrder");

            entity.HasOne(d => d.Customer).WithMany(p => p.DeOrders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DeOrder_DeUser");

            entity.HasOne(d => d.PickupPoint).WithMany(p => p.DeOrders)
                .HasForeignKey(d => d.PickupPointId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DeOrder_DePickupPoint");
        });

        modelBuilder.Entity<DeOrderHasDeProduct>(entity =>
        {
            entity.HasKey(e => new { e.ProductId, e.OrderId }).HasName("PK_DeOrderHasDeProduct_1");

            entity.ToTable("DeOrderHasDeProduct");

            entity.Property(e => e.ProductId).HasMaxLength(6);

            entity.HasOne(d => d.Order).WithMany(p => p.DeOrderHasDeProducts)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DeOrderHasDeProduct_DeOrder");

            entity.HasOne(d => d.Product).WithMany(p => p.DeOrderHasDeProducts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DeOrderHasDeProduct_DeProduct");
        });

        modelBuilder.Entity<DePickupPoint>(entity =>
        {
            entity.ToTable("DePickupPoint");

            entity.Property(e => e.City).HasMaxLength(150);
            entity.Property(e => e.Street).HasMaxLength(150);
        });

        modelBuilder.Entity<DeProduct>(entity =>
        {
            entity.ToTable("DeProduct");

            entity.Property(e => e.Id).HasMaxLength(6);
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Photo).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(7, 2)");
            entity.Property(e => e.ProductName).HasMaxLength(150);

            entity.HasOne(d => d.Manufacturer).WithMany(p => p.DeProducts)
                .HasForeignKey(d => d.ManufacturerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DeProduct_DeManufacturer");

            entity.HasOne(d => d.Supplier).WithMany(p => p.DeProducts)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DeProduct_DeSupplier");
        });

        modelBuilder.Entity<DeRole>(entity =>
        {
            entity.ToTable("DeRole");

            entity.Property(e => e.Name).HasMaxLength(20);
        });

        modelBuilder.Entity<DeSupplier>(entity =>
        {
            entity.ToTable("DeSupplier");

            entity.Property(e => e.Supplier).HasMaxLength(150);
        });

        modelBuilder.Entity<DeUser>(entity =>
        {
            entity.ToTable("DeUser");

            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.Login).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(150);
            entity.Property(e => e.Patronymic).HasMaxLength(100);

            entity.HasOne(d => d.Role).WithMany(p => p.DeUsers)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DeUser_DeRole");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
