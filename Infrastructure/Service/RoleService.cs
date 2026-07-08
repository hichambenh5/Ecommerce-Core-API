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
   public class RoleService:IRoleService
    {
        private readonly IRoleRepository _repo;
        public RoleService(IRoleRepository repo)
        {
            _repo = repo;
        }
        private RoleDto MapToRoleDto(Role role)
        {
           return new RoleDto
            {
                RoleId = role.RoleId,
                RoleName = role.RoleName,
                NumberPermissions = role.NumberPermissions
            };
        }
        public async Task<RoleDto?> GetByIdAsync(int id)
        {
            var role = await _repo.GetByIdAsync(id);
            return role == null ? null : MapToRoleDto(role);
        }
        public async Task<IEnumerable<RoleDto>> GetAllAsync()
        {
            var roles = await _repo.GetAllAsync();
            return roles.Select(MapToRoleDto);
        }
        public async Task<int> AddAsync(CreateRoleDto createDto)
        {
            try
            {
                var role = new Role { RoleName = createDto.RoleName,NumberPermissions=createDto.NumberPermissions };

              return  await _repo.AddAsync(role);
            }catch(Exception ex)
            {
                throw new Exception("An error occurred while saving the Role to the database. Please try again later.", ex);
            }
        }
        public async Task<bool> UpdateAsync(int id,UpdateRoleDto updateDto)
        {
            try
            {
                var role = await _repo.GetByIdAsync(id);
                if (role == null) return false;
                role.RoleName = updateDto.RoleName;
                role.NumberPermissions = updateDto.NumberPermissions;
                return await _repo.UpdateAsync(role);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating Role with ID {id}.", ex);
            }
        }
        public async Task<bool> DeleteAsync(int id)
        {
            try {
                return await _repo.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting Role with ID {id}.", ex);
            }
        }
        public async Task<bool> RoleExistsAsync(string roleName) =>await _repo.RoleExistsAsync(roleName);
    }
}
