using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class RoleRepository:IRoleRepository
    {
        private readonly EcommerceDbContext _context;
public RoleRepository(EcommerceDbContext context)
        {
            _context = context;
        }
        public async Task<Role?> GetByIdAsync(int id)
        {
            return await _context.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.RoleId == id);
        }
        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            return await _context.Roles.AsNoTracking().ToListAsync();
        }
        public async Task<int> AddAsync(Role role)
        {
            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();
            return role.RoleId;
        }
        public async Task<bool> UpdateAsync(Role role)
        {
           var roles= await _context.Roles.FindAsync(role.RoleId);
            if (roles == null) return false;
            MappingExtensions.PatchValues(roles, role);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var roles = await _context.Roles.FindAsync(id);
            if (roles == null) return false;
            _context.Roles.Remove(roles);
            await _context.SaveChangesAsync();
            return true;

        }
        public async Task<bool> RoleExistsAsync(string roleName)
        {
            return await _context.Roles.AnyAsync(r => r.RoleName == roleName);
        }

    }
}
