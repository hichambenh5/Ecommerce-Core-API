using Infrastructure.DTOs;
using Infrastructure.Models;
using Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Service
{
    public class ShoppingCartService:IShoppingCartService
    {
        private readonly IShoppingCartRepository _cartRepo;

        public ShoppingCartService(IShoppingCartRepository cartRepo)
        {
            _cartRepo = cartRepo;
        }
        private CartItemDto MapToCartItemDto(ShoppingCart cartItem)
        {
            return new CartItemDto
            {
                ShoppingCartId = cartItem.ShoppingCartId,
                UserId = cartItem.UserId ?? 0,    
                VariantId = cartItem.VariantId ?? 0,
                Quantity = cartItem.Quantity
            };
        }

        public async Task<IEnumerable<CartItemDto>> GetUserCartAsync(int userId)
        {
            try
            {
                var cartItems = await _cartRepo.GetCartItemsByUserIdAsync(userId);
                return cartItems.Select(MapToCartItemDto);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the cart for user ID {userId}.", ex);
            }
        }
        public async Task<int> CreateCartAsync(int userId, AddToCartDto dto)
        {
            try
            {
                var cartItem = new ShoppingCart
                {
                    UserId = userId,
                    VariantId = dto.VariantId,
                    Quantity = dto.Quantity
                };

                return await _cartRepo.CreateCartAsync(cartItem);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the cart item.", ex);
            }
        }
        public async Task AddItemToCartAsync(int userId, AddToCartDto dto)
        {
            try
            {
                await _cartRepo.AddOrUpdateItemAsync(userId, dto.VariantId, dto.Quantity);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the item to the shopping cart.", ex);
            }
        }

        public async Task<bool> UpdateItemQuantityAsync(int shoppingCartId, UpdateCartItemDto dto)
        {
            try
            {
                if (dto.Quantity <= 0)
                {
                    await _cartRepo.RemoveItemAsync(shoppingCartId);
                    return true;
                }

                return await _cartRepo.UpdateQuantityAsync(shoppingCartId, dto.Quantity);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating shopping cart item ID {shoppingCartId}.", ex);
            }
        }

        public async Task RemoveCartItemAsync(int shoppingCartId)
        {
            try
            {
                await _cartRepo.RemoveItemAsync(shoppingCartId);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while removing cart item ID {shoppingCartId}.", ex);
            }
        }

        public async Task<bool> ClearUserCartAsync(int userId)
        {
            try
            {
                return await _cartRepo.ClearCartAsync(userId);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while clearing the cart for user ID {userId}.", ex);
            }
        }
    
}
}
