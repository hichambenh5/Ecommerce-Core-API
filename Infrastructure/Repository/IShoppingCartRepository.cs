using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public interface IShoppingCartRepository
    {
        Task<IEnumerable<ShoppingCart>> GetCartItemsByUserIdAsync(int userId);
        Task<int> CreateCartAsync(ShoppingCart cart);

        Task AddOrUpdateItemAsync(int userId, int variantId, int quantity);

        Task <bool>UpdateQuantityAsync(int shoppingCartId, int quantity);

        Task RemoveItemAsync(int shoppingCartId);

       
        Task <bool>ClearCartAsync(int userId);

       
    }
}
