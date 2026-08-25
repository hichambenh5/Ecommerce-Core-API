using Infrastructure.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Service
{
    public interface IShoppingCartService
    {
        Task<IEnumerable<CartItemDto>> GetUserCartAsync(int userId);
        Task<int> CreateCartAsync(int userId, AddToCartDto dto);

        Task AddItemToCartAsync(int userId, AddToCartDto addToCartDto);

        Task<bool> UpdateItemQuantityAsync(int shoppingCartId, UpdateCartItemDto updateDto);

        Task RemoveCartItemAsync(int shoppingCartId);

        Task<bool> ClearUserCartAsync(int userId);

    }
}
