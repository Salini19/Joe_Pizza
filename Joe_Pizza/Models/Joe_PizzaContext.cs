using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Joe_Pizza.Models
{
    public partial class Joe_PizzaContext : DbContext
    {
        public Joe_PizzaContext()
        {
        }

        public Joe_PizzaContext(DbContextOptions<Joe_PizzaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Invoice> Invoices { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<Pizza> Pizzas { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-1I1KGJE\\SQLEXPRESS01;database=Joe_Pizza;trusted_Connection=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.ToTable("Invoice");

                entity.Property(e => e.InvUser).HasColumnName("Inv_User");

                entity.Property(e => e.InvoiceDate).HasColumnType("datetime");

                entity.HasOne(d => d.InvUserNavigation)
                    .WithMany(p => p.Invoices)
                    .HasForeignKey(d => d.InvUser)
                    .HasConstraintName("FK__Invoice__Inv_Use__3C69FB99");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.HasOne(d => d.InvoiceNoNavigation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.InvoiceNo)
                    .HasConstraintName("FK__Orders__InvoiceN__403A8C7D");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK__Orders__ProductI__3F466844");
            });

            modelBuilder.Entity<Pizza>(entity =>
            {
                entity.HasKey(e => e.ProductId)
                    .HasName("PK__Pizza__B40CC6CD86190179");

                entity.ToTable("Pizza");

                entity.HasIndex(e => e.ProductName, "UQ__Pizza__DD5A978AA3D53B0D")
                    .IsUnique();

                entity.Property(e => e.ProductDescription).HasMaxLength(500);

                entity.Property(e => e.ProductName).HasMaxLength(50);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UId)
                    .HasName("PK__Users__B51D3DEA3B3104AA");

                entity.HasIndex(e => e.Contact, "UQ__Users__F7C046659897F2A5")
                    .IsUnique();

                entity.Property(e => e.UId).HasColumnName("u_id");

                entity.Property(e => e.Contact).HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.Username).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
