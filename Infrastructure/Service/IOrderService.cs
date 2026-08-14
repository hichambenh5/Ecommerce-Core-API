using Infrastructure.DTOs;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Service
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderResponseDto>> GetAllOrdersAsync();
        Task<OrderResponseDto?> GetOrderByIdAsync(int id);
        Task<int> AddOrderAsync(OrderCreateDto order);
        Task<bool> UpdateOrderAsync(int id,OrderUpdateDto order);
        Task<bool> DeleteOrderAsync(int id);
        Task<bool> ExistsOrderAsync(int id);
        Task<IEnumerable<OrderResponseDto>> GetOrdersByUserIdAsync(int userId);
        Task<bool> UpdateOrderStatusAsync(int id, string newStatus);
    }
}
