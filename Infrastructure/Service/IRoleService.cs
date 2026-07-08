using Infrastructure.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Service
{
    public interface IRoleService
    {
        Task<RoleDto?> GetByIdAsync(int id);
        Task<IEnumerable<RoleDto>> GetAllAsync();
        Task<int> AddAsync(CreateRoleDto createDto);
        Task<bool> UpdateAsync(int id,UpdateRoleDto updateDto);
        Task<bool> DeleteAsync(int id);
        Task<bool> RoleExistsAsync(string roleName);
    }
}
