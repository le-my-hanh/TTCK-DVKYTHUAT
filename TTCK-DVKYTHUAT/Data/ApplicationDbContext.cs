using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TTCK_DVKYTHUAT.Models;

namespace TTCK_DVKYTHUAT.Data;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<CategoryNews> CategoryNews { get; set; }

    public virtual DbSet<Conment> Conments { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Reply> Replys { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<TransactStatus> TransactStatuses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=MYHANH\\SQLEXPRESS;Database=DVKYTHUAT;Integrated Security=True;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Conment>(entity =>
        {
            entity.HasOne(d => d.Customer).WithMany(p => p.Conments)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Conment_Customers");

            entity.HasOne(d => d.Service).WithMany(p => p.Conments)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Conment_Services");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.Property(e => e.Salt).IsFixedLength();
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.HasOne(d => d.Categorynew).WithMany(p => p.News)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_News_CategoryNews");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Orders_Customers");

            entity.HasOne(d => d.Payment).WithMany(p => p.Orders)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Orders_Payment");

            entity.HasOne(d => d.TransactStatus).WithMany(p => p.Orders)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Orders_TransactStatus");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_OrderDetails_Orders");

            entity.HasOne(d => d.Service).WithMany(p => p.OrderDetails)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_OrderDetails_Services");
        });

        modelBuilder.Entity<Reply>(entity =>
        {
            entity.HasOne(d => d.Conment).WithMany(p => p.Replies)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Replys_Conment");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasOne(d => d.Category).WithMany(p => p.Services)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Services_Categories");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
