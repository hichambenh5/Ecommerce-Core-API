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
    public class AddressService:IAddressService
    {
        private readonly IAddressRepository _Repo;
        public AddressService(IAddressRepository repo)
        {
            _Repo = repo;
        }
        private AddressDto MapToAddressDto(Address address)
        {
            return new AddressDto
            {
                AddressId = address.AddressId,
                City = address.City,
                StreetAddress = address.StreetAddress,
                PostalCode = address.PostalCode
            };
        }
        public async Task<IEnumerable<AddressDto>> GetUserAddressesAsync(int userId)
        {
            var address = await _Repo.GetUserAddressesAsync(userId);
            return address.Select(MapToAddressDto);
        }
        public async Task<AddressDto?> GetAddressByIdAsync(int addressId, int userId)
        {
            var address = await _Repo.GetByIdAsync(addressId, userId);
            return address == null ? null : MapToAddressDto(address);
        }
        public async Task<int> AddAddressAsync(CreateAddressDto createDto)
        {
            try {
                var address = new Address
                {
                    
                    City = createDto.City,
                    StreetAddress = createDto.StreetAddress,
                    PostalCode = createDto.PostalCode,
                    UserId=createDto.UserId

                    
                };
                return await _Repo.AddAsync(address);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while saving the Address to the database. Please try again later.", ex);
            }
        }
        public async Task<bool> UpdateAddressAsync(int userId, int addressId, UpdateAddressDto updateDto)
        {
            try
            {
                var address = await _Repo.GetByIdAsync(addressId, userId);
                if (address == null) return false;

                MappingExtensions.PatchValues(address, updateDto);
                return await _Repo.UpdateAsync(address);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating Address with ID {addressId}.", ex);
            }
        }
        public async Task<bool> DeleteAddressAsync(int addressId)
        {
            try
            {
                return await _Repo.DeleteAsync(addressId); 
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting Address with ID {addressId}.", ex);
            }
        }
    }
}
