using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
  public class AddressRepository:IAddressRepository
    {
        private readonly EcommerceDbContext _context;
        public AddressRepository(EcommerceDbContext context)
        {
            _context = context;
        }
       public async Task<List<Address>> GetUserAddressesAsync(int userId)
        {
            return await _context.Addresses.Where(u => u.UserId == userId).AsNoTracking().ToListAsync();
        }
        public async Task<Address?> GetByIdAsync(int addressId, int userId)
        {
            return await _context.Addresses.AsNoTracking().FirstOrDefaultAsync(a => a.AddressId == addressId && a.UserId == userId);
        }
        public async Task<int> AddAsync(Address address)
        {
            await _context.Addresses.AddAsync(address);
            await _context.SaveChangesAsync();
            return address.AddressId;
        }
        public async Task<bool> UpdateAsync(Address addressupdate)
        {
            var address = await _context.Addresses.FindAsync(addressupdate.AddressId);
            if (address == null) return false;
            MappingExtensions.PatchValues(address, addressupdate);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteAsync(int addressid)
        {
            var address = await _context.Addresses.FindAsync(addressid);
            if (address == null) return false;
            _context.Remove(address);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
