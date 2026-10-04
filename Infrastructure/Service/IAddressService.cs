using Infrastructure.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Service
{
   public interface IAddressService
    {
        Task<IEnumerable<AddressDto>> GetUserAddressesAsync(int userId);
        Task<AddressDto?> GetAddressByIdAsync(int addressId, int userId);
        Task<int> AddAddressAsync(CreateAddressDto createDto);
        Task<bool> UpdateAddressAsync(int userId, int addressId, UpdateAddressDto updateDto);
        Task<bool> DeleteAddressAsync(int addressId);
    }
}
