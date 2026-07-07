using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public interface IRoleRepository
    {
        Task<Role?> GetByIdAsync(int id);
        Task<IEnumerable<Role>> GetAllAsync();

      
        Task<int> AddAsync(Role role);

        Task<bool> UpdateAsync(Role role);
        Task<bool> DeleteAsync(int id);

        Task<bool> RoleExistsAsync(string roleName);
    }
}
