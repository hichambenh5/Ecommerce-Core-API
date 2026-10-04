using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public interface IAddressRepository
    {
        Task<List<Address>> GetUserAddressesAsync(int userId);
        Task<Address?> GetByIdAsync(int addressId, int userId);
        Task<int> AddAsync(Address address);
        Task <bool>UpdateAsync(Address address);
        Task <bool>DeleteAsync(int addressid);
    }
}
