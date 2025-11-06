using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Core.Entities;
using Infrastructure.Config;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class StoreContext(DbContextOptions options) : IdentityDbContext<AppUser>(options)
    {
        public DbSet<Product> Products { get; set; }

        public DbSet<ShoppingCart> Carts { get; set; }

        public DbSet<CartItem> CartItems { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<DeliveryMethod> DeliveryMethods { get; set;}


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductConfiguration).Assembly);

            modelBuilder.Entity<ShoppingCart>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<CartItem>()
                .HasKey(x => new { x.CartId, x.ProductId });

            modelBuilder.Entity<CartItem>()
                .Property(x => x.ProductId)
                .ValueGeneratedNever();

            modelBuilder.Entity<CartItem>()
                .Property(x => x.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<ShoppingCart>()
                .HasMany(x => x.Items)
                .WithOne(x => x.Cart)
                .HasForeignKey(x => x.CartId)
                .IsRequired();
        }
    }
}