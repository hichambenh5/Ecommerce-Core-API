using Infrastructure.DTOs;
using Infrastructure.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CoreCommerce_API.Controllers
{
    [Route("api/Address")]
    [ApiController]
    public class AddressApi : ControllerBase
    {
        private readonly IAddressService _Addressservice;
        public AddressApi(IAddressService addressService)
        {
            _Addressservice = addressService;
        }
        [HttpGet("{userId}", Name = "GetUserAddressesAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<AddressDto>>> GetUserAddressesAsync(int userId)
        {
            if (userId < 1)
            {
                return BadRequest($"Not Actepted id: {userId}");
            }
            var Address = await _Addressservice.GetUserAddressesAsync(userId);
            if (Address == null || !Address.Any())
            {
                return NotFound("Address Not Found");
            }
            return Ok(Address);
        }
        [HttpGet("{addressId}/{userId}", Name = "GetAddressByIdAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AddressDto?>> GetAddressByIdAsync(int addressId, int userId)
        {
            if (userId < 1 || addressId < 1) {
                return BadRequest($"Not Actepted ids: {addressId} and {userId}");
            }
            var Address = await _Addressservice.GetAddressByIdAsync(addressId, userId);
            if (Address == null)
            {
                return NotFound("Address Not Found");
            }
            return Ok(Address);

        }
        [HttpPost(Name = "AddAddressAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CreateAddressDto>> AddAddressAsync(CreateAddressDto createDto)
        {
            if (createDto == null || string.IsNullOrEmpty(createDto.City) ||
                string.IsNullOrEmpty(createDto.StreetAddress) ||
                string.IsNullOrEmpty(createDto.PostalCode) || createDto.UserId < 1)
            {
                return BadRequest("Invalid Adresses Data");
            }
            var AddressCreate = await _Addressservice.AddAddressAsync(createDto);
            if (AddressCreate <= 0)
            {
                return BadRequest("Error Creating Address");
            }
            var response = new CreateAddressDto
            {
                City = createDto.City,
                StreetAddress = createDto.StreetAddress,
                PostalCode = createDto.PostalCode,
                UserId = createDto.UserId
            };
            return CreatedAtRoute("GetAddressByIdAsync", new { AddressId = AddressCreate, UserId = createDto.UserId }, response);
        }
        [HttpPut("{AddressId}/{userId}",Name = "UpdateAddressAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UpdateAddressDto>> UpdateAddressAsync(int AddressId,int userId, UpdateAddressDto updateDto)
        {
            if (updateDto == null || string.IsNullOrEmpty(updateDto.City) ||
               string.IsNullOrEmpty(updateDto.StreetAddress) ||
               string.IsNullOrEmpty(updateDto.PostalCode))
            {
                return BadRequest("Invalid Adresses Data");
            }
            var AddressUpdate = await _Addressservice.GetAddressByIdAsync(AddressId, userId);
            if (AddressUpdate == null)
            {
                return NotFound($"Address with id {AddressId} not found");
            }
            var isUpdated = await _Addressservice.UpdateAddressAsync(userId, AddressId, updateDto);

            if (!isUpdated)
            {
                return NotFound($"Address with id {AddressId} not found");
            }
            return Ok(updateDto);

        }
        [HttpDelete("{addressId}",Name = "DeleteAddressAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteAddressAsync(int addressId)
        {
            if (addressId < 1)
            {
                return BadRequest("Invalid Adresses Data");
            }
            if (await _Addressservice.DeleteAddressAsync(addressId))
            {
                return Ok($"Address with id {addressId} has been deleted");
            }
            else
            {
                return NotFound($"Address with id {addressId} not found,no rows deleted");
            }
        }
    }
}
