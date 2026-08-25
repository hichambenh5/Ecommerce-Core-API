using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class ShoppingCartRepository: IShoppingCartRepository
    {
        private readonly EcommerceDbContext _context;
        public ShoppingCartRepository(EcommerceDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ShoppingCart>> GetCartItemsByUserIdAsync(int userId)
        {
            return await _context.ShoppingCarts
         .Where(sc => sc.UserId == userId)
         .Include(sc => sc.Variant) 
         .AsNoTracking()
         .ToListAsync();
        }
        public async Task<int> CreateCartAsync(ShoppingCart cart)
        {
            await _context.ShoppingCarts.AddAsync(cart);
            await _context.SaveChangesAsync();
            return cart.ShoppingCartId;
        }
        public async Task AddOrUpdateItemAsync(int userId, int variantId, int quantity)
        {
            var existingItem = await _context.ShoppingCarts.FirstOrDefaultAsync(sc => sc.UserId == userId && sc.VariantId == variantId);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                _context.ShoppingCarts.Update(existingItem);
            }
            else
            {
                var newItem = new ShoppingCart
                {
                    UserId = userId,
                    VariantId = variantId,
                    Quantity = quantity
                };
                await _context.ShoppingCarts.AddAsync(newItem);
            }
            await _context.SaveChangesAsync();
        }
        public async Task<bool> UpdateQuantityAsync(int shoppingCartId, int quantity)
        {
            var shoppingCart = await _context.ShoppingCarts.FindAsync(shoppingCartId);
            if (shoppingCart == null) return false;
            shoppingCart.Quantity = quantity;
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task RemoveItemAsync(int shoppingCartId)
        {
            var cartItem = await _context.ShoppingCarts.FindAsync(shoppingCartId);

            if (cartItem != null)
            {
                _context.ShoppingCarts.Remove(cartItem);
                await _context.SaveChangesAsync();
               
            }
        }
        public async Task<bool> ClearCartAsync(int userId)
        {
            var userItems = await _context.ShoppingCarts
         .Where(sc => sc.UserId == userId)
         .ToListAsync();

            if (!userItems.Any()) return false; 

            _context.ShoppingCarts.RemoveRange(userItems);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
