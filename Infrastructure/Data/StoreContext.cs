using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Core.Entities;
using Infrastructure.Config;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class StoreContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Product> Products { get; set; }

        public DbSet<ShoppingCart> Carts { get; set; }

        public DbSet<CartItem> CartItems { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductConfiguration).Assembly);

            modelBuilder.Entity<ShoppingCart>().HasKey(x => x.Id);  

            modelBuilder.Entity<CartItem>().HasKey(x => new {x.CartId, x.ProductId});

            modelBuilder.Entity<CartItem>().Property(x => x.ProductId).
            ValueGeneratedNever();

            modelBuilder.Entity<ShoppingCart>().HasMany(x => x.Items).
                WithOne().HasForeignKey(x => x.CartId).IsRequired();
        }
    }
}