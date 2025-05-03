namespace WebApplication2.DataAccess;

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using WebApplication2.Models;

public class WarehouseContext : DbContext
{
    public WarehouseContext(DbContextOptions<WarehouseContext> options)
        : base(options)
    {
    }

    public DbSet<Client> Clients { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Box> Boxes { get; set; }
    public DbSet<BoxFlower> BoxFlowers { get; set; }
    public DbSet<Flower> Flowers { get; set; }
    public DbSet<Dye> Dyes { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Order-Status enum storage
        modelBuilder.Entity<Order>()
            .Property(o => o.Status)
            .HasConversion<string>();

        // Configure Flower-Ingredient many-to-many
        modelBuilder.Entity<Flower>()
    .HasMany(f => f.Ingredients)
    .WithMany(i => i.Flowers)
    .UsingEntity<Dictionary<string, object>>(
        "flower_ingredient",
        j => j.HasOne<Ingredient>().WithMany().HasForeignKey("IngredientId"),
        j => j.HasOne<Flower>().WithMany().HasForeignKey("FlowerId")
    );

        // Configure Box-Flower join table composite key
        modelBuilder.Entity<BoxFlower>()
            .HasKey(bf => new { bf.BoxId, bf.FlowerId });

        // Configure relationships
        modelBuilder.Entity<Order>()
            .HasMany(o => o.Boxes)
            .WithOne(b => b.Order)
            .HasForeignKey(b => b.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Box>()
            .HasMany(b => b.Flowers)
            .WithOne(bf => bf.Box)
            .HasForeignKey(bf => bf.BoxId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Flower>()
            .HasOne(f => f.Dye)
            .WithMany()
            .HasForeignKey(f => f.DyeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}