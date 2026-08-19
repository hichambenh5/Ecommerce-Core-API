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
    public class OrderService:IOrderService
    {
        private readonly IOrderRepository _repo;
        public OrderService(IOrderRepository repo)
        {
            _repo = repo;
        }
        private OrderResponseDto MapToOrderDto(Order order)
        {
            return new OrderResponseDto
            {
                OrderId = order.OrderId,
                OrderDate = order.OrderDate,
                TotalPrice=order.TotalPrice,
                OrderStatus=order.OrderStatus,
                UserId=order.UserId,
                CouponsId=order.CouponsId,
                OrderItems = order.OrderItems?.Select(oi => new OrderItemResponseDto
                {
                    OrderItemId = oi.OrderItemId,
                    Quantity = oi.Quantity,
                    Price = oi.Price,
                    VariantId = oi.VariantId
                }).ToList() ?? new List<OrderItemResponseDto>()
            };
        }
        public async Task<IEnumerable<OrderResponseDto>> GetAllOrdersAsync()
        {
            var orders = await _repo.GetAllOrdersAsync();
            return orders.Select(MapToOrderDto);
        }
        public async Task<OrderResponseDto?> GetOrderByIdAsync(int id)
        {
            var order = await _repo.GetOrderByIdAsync(id);
            return order == null ? null : MapToOrderDto(order);
        }
        public async Task<int> AddOrderAsync(OrderCreateDto createorder)
        {
            try
            {
              

                   var order = new Order
        {
            TotalPrice = createorder.TotalPrice,
            UserId = createorder.UserId,
            CouponsId = createorder.CouponsId,
            OrderStatus = "Pending", 
            OrderDate = DateTime.UtcNow,
                       OrderItems = createorder.OrderItems.Select(oi => new OrderItem
                       {
                           VariantId = oi.VariantId,
                           Quantity = oi.Quantity,
                           Price = oi.Price
                       }).ToList()
                   };
                
                return await _repo.AddOrderAsync(order);
            }catch(Exception ex)
            {
                throw new Exception("An error occurred while saving the Order to the database. Please try again later.", ex);
            }
        }
        public async Task<bool> UpdateOrderAsync(int id,OrderUpdateDto dto)
        {
            try
            {
                var order = await _repo.GetOrderByIdAsync(id);
                if (order == null) return false;

                if (!string.IsNullOrEmpty(dto.OrderStatus))
                {
                    order.OrderStatus = dto.OrderStatus;
                }
                if (dto.TotalPrice.HasValue)
                {
                    order.TotalPrice = dto.TotalPrice.Value;
                }
                    
               
                return await _repo.UpdateOrderAsync(order);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating Order with ID {id}.", ex);
            }
        }
        public async Task<bool> DeleteOrderAsync(int id)
        {
            try
            {
                return await _repo.DeleteOrderAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting Order with ID {id}.", ex);
            }
        }
        public async Task<bool> ExistsOrderAsync(int id) => await _repo.ExistsOrderAsync(id);
        public async Task<IEnumerable<OrderResponseDto>> GetOrdersByUserIdAsync(int userId)
        {
            var orders = await _repo.GetOrdersByUserIdAsync(userId);
            return orders.Select(MapToOrderDto);
        }
        public async Task<bool> UpdateOrderStatusAsync(int id, string newStatus)
        {
            try
            {
                return await _repo.UpdateOrderStatusAsync(id, newStatus);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating status for Order with ID {id}.", ex);
            }
        }
    }
}
