using Infrastructure.DTOs;
using Infrastructure.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CoreCommerce_API.Controllers
{
    [Route("api/Role")]
    [ApiController]
    public class RoleApi : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RoleApi(IRoleService roleService)
        {
            _roleService = roleService;
        }
        [HttpGet("{id}", Name = "GetByIdAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RoleDto?>> GetByIdAsync(int id)
        {
            if (id < 1)
            {
                return BadRequest($"Not Actepted id: {id}");
            }
            var role = await _roleService.GetByIdAsync(id);
            if (role == null)
            {
                return NotFound($"Role with id {id} Notfound");
            }
            return Ok(role);
        }
        [HttpGet("GetAll", Name = "GetAllAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<RoleDto>>> GetAllAsync()
        {
            var rolelist = await _roleService.GetAllAsync();
            if(rolelist==null || !rolelist.Any())
            {
                return NotFound("Roles not found");
            }
            return Ok(rolelist);
        }
        [HttpPost(Name = "AddAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CreateRoleDto>> AddAsync(CreateRoleDto createDto)
        {
            if(createDto==null || string.IsNullOrEmpty(createDto.RoleName))
            {
                return BadRequest("invalid Role data");
            }
            var rolecreate = await _roleService.AddAsync(createDto);
            if (rolecreate < 1)
            {
                return BadRequest("Error creating Role");
            }
            var response = new CreateRoleDto
            {
                RoleName = createDto.RoleName,
                NumberPermissions = createDto.NumberPermissions
            };
            return CreatedAtRoute("GetByIdAsync", new { id = rolecreate }, response);
        }
        [HttpDelete("{id}", Name = "DeleteAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            if (id < 1)
            {
                return BadRequest("invalid Role data");
            }
            if(await _roleService.DeleteAsync(id))
            {
                return Ok($"Role with id {id} has been deleted");
            }
            else
            {
                return NotFound($"Role with id {id} not found,no rows deleted");
            }
        }
        [HttpPut("{id}", Name = "UpdateAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UpdateRoleDto>> UpdateAsync(int id, UpdateRoleDto updateDto)
        {
            if (id<1 || updateDto == null || string.IsNullOrEmpty(updateDto.RoleName))
            {
                return BadRequest("invalid Role data");
            }
            var Role = await _roleService.GetByIdAsync(id);
            if (Role == null)
            {
                return NotFound($"Role with id {id} not found");
            }
            Role.RoleName = updateDto.RoleName;
            Role.NumberPermissions = updateDto.NumberPermissions;
            if(await _roleService.UpdateAsync(id, updateDto))
            {
                return Ok(updateDto);
            }
            else
            {
                return StatusCode(500, "update errer");
            }
        }
        [HttpHead("{id}", Name = "RoleExistsAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> RoleExistsAsync(string roleName)
        {
            var Exists = await _roleService.RoleExistsAsync(roleName);
            if (Exists)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
