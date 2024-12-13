using AutoMapper;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebLoginBLL.DTO;

namespace WebLoginBLL.Services
{
    public class RoleService
    {
        private readonly BankContext _context;
        private readonly IMapper _mapper;

        public RoleService(BankContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RoleDTO>> GetAllRolesAsync()
        {
            var roles = await _context.Roles.ToListAsync();
            return _mapper.Map<IEnumerable<RoleDTO>>(roles);
        }

        public async Task<RoleDTO?> GetRoleByIdAsync(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            return role == null ? null : _mapper.Map<RoleDTO>(role);
        }

        public async Task CreateRoleAsync(RoleDTO roleDto)
        {
            var role = _mapper.Map<Role>(roleDto);
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRoleAsync(RoleDTO roleDto)
        {
            var role = _mapper.Map<Role>(roleDto);
            _context.Roles.Update(role);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRoleAsync(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role != null)
            {
                _context.Roles.Remove(role);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<UserDTO>> GetUsersInRoleAsync(int roleId)
        {
            var users = await _context.Users
                .Where(u => u.RoleId == roleId)
                .ToListAsync();
            return _mapper.Map<IEnumerable<UserDTO>>(users);
        }


    }
}
