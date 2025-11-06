using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace Infrastructure.Services
{
    public class CartService(StoreContext context) : ICartService
    {
        public async Task<bool> DeleteCartAsync(string key)
        {
            var cart = await context.Carts.Include(x => x.Items).
                FirstOrDefaultAsync(x => x.Id == key);
            if (cart == null) return false;

            context.Carts.Remove(cart);

            await context.SaveChangesAsync();
            return true;
        }

        public async Task<ShoppingCart?> GetCartAsync(string key)
        {
            return await context.Carts.Include(x => x.Items).
                FirstOrDefaultAsync(x => x.Id == key);
        }

        public async Task<ShoppingCart?> SetCartAsync(ShoppingCart cart)
        {
            var existingCart = await context.Carts.Include(x => x.Items).
                FirstOrDefaultAsync(x => x.Id == cart.Id);
            if (existingCart == null)
            {
                foreach (var item in cart.Items)
                {
                    item.CartId = cart.Id;
                }
                context.Carts.Add(cart);
            }
            else
            {
                //update basic cart properties 
                context.Entry(existingCart).CurrentValues.SetValues(cart);

                var itemsToRemove = existingCart.Items.Where(i => !cart.Items.Any
                (ci => ci.ProductId == i.ProductId)).ToList();

                foreach (var itemToRemove in itemsToRemove)
                {
                    existingCart.Items.Remove(itemToRemove);
                }

                //Handle cart items
                if (cart.Items != null && cart.Items.Any())
                {

                    foreach (var item in cart.Items)
                    {
                        var existingItem = existingCart.Items.FirstOrDefault
                        (x => x.ProductId == item.ProductId);

                        if (existingItem != null)
                        {
                            existingItem.Quantity = item.Quantity;
                            existingItem.Price = item.Price;
                        }
                        else
                        {
                            //Add new item
                            item.CartId = cart.Id;
                            existingCart.Items.Add(item);
                        }
                    }
                }
            }                
            await context.SaveChangesAsync();

            return await GetCartAsync(cart.Id);
        }
    }
}