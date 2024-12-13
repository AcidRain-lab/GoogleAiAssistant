// RoleManager.cs in BLL namespace

using DAL.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BLL
{
    public class RoleManager
    {
        private readonly CrmContext _context;

        public RoleManager(CrmContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<Role>> GetAllRoles()
        {
            // Retrieves all roles from the database
            return await _context.Roles.ToListAsync();
        }



        // Business logic for creating a role
        public async Task<bool> CreateRole(Role role)
        {
            if (_context.Roles.Any(r => r.Name == role.Name))
            {
                // Role with the same name already exists
                return false;
            }

            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
            return true;
        }

        // Business logic for editing a role
        public async Task<bool> EditRole(int id, Role role)
        {
            if (id != role.Id)
            {
                // Role not found or id mismatch
                return false;
            }

            try
            {
                _context.Update(role);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                // Handle concurrency issues or role not found
                return false;
            }
        }

        // Business logic for deleting a role
        public async Task<bool> DeleteRole(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null)
            {
                // Role not found
                return false;
            }
            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
            return true;
        }

        // Business logic for getting a role by ID
        public async Task<Role> GetRoleById(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            return role; // This could be null if role doesn't exist
        }

        // Business logic to check if a role exists
        public bool RoleExists(int id)
        {
            return _context.Roles.Any(e => e.Id == id);
        }

      
    }
}
