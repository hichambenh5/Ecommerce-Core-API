using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class OrderRepository:IOrderRepository
    {
        private readonly EcommerceDbContext _context;
        public OrderRepository(EcommerceDbContext context)
        {
            _context = context;
        }
        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders.AsNoTracking().ToListAsync();
        }
        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await _context.Orders.AsNoTracking().FirstOrDefaultAsync(o => o.OrderId == id);
        }
        public async Task<int> AddOrderAsync(Order order)
        {
             await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return order.OrderId;

        }
        public async Task<bool> UpdateOrderAsync(Order updateorder)
        {
           var order= await _context.Orders.FindAsync(updateorder.OrderId);
            if (order == null) return false;
            MappingExtensions.PatchValues(order, updateorder);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteOrderAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return false;
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return true;

        }
        public async Task<bool> ExistsOrderAsync(int id)
        {
            return await _context.Orders.AnyAsync(o => o.OrderId == id);
        }
      
    }
}
