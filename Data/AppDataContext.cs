using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace H5ServersideProgrammering.Data;

public partial class AppDataContext : DbContext
{
    public AppDataContext()
    {
    }

    public AppDataContext(DbContextOptions<AppDataContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TodoItem> TodoItems { get; set; }

    public virtual DbSet<UserCpr> UserCprs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TodoItem>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ItemText).HasColumnType("nvarchar(500)");
            entity.Property(e => e.UserId)
                .HasColumnType("nvarchar(500)")
                .HasColumnName("UserID");
        });

        modelBuilder.Entity<UserCpr>(entity =>
        {
            entity.ToTable("UserCPR");

            entity.HasIndex(e => e.UserId, "IX_UserCPR_UserID").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Cpr)
                .HasColumnType("nvarchar(500)")
                .HasColumnName("CPR");
            entity.Property(e => e.UserId)
                .HasColumnType("nvarchar(500)")
                .HasColumnName("UserID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
