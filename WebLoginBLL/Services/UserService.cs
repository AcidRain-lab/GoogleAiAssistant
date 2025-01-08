using AutoMapper;
using DAL.Models;
using WebLoginBLL.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAuthCoreBLL.Helpers;

namespace WebLoginBLL.Services
{
    public class UserService
    {
        private readonly BankContext _context;
        private readonly IMapper _mapper;

        public UserService(BankContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Валидация пользователя по имени и паролю.
        /// </summary>
        public async Task<UserDTO?> ValidateUserAsync(string userName, string password)
        {
            var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.UserName == userName);
            if (user != null && SecurePasswordHasher.Verify(password, user.PasswordHash))
            {
                return _mapper.Map<UserDTO>(user);
            }
            return null;
        }

        /// <summary>
        /// Получение всех пользователей с их ролями.
        /// </summary>
        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            var users = await _context.Users.Include(u => u.Role).ToListAsync();
            return _mapper.Map<IEnumerable<UserDTO>>(users);
        }

        /// <summary>
        /// Получение пользователя по его ID.
        /// </summary>
        public async Task<UserDTO?> GetUserByIdAsync(Guid id)
        {
            var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == id);
            return user == null ? null : _mapper.Map<UserDTO>(user);
        }

        /// <summary>
        /// Создание нового пользователя.
        /// </summary>
        public async Task<(bool success, string errorMessage)> CreateUserAsync(UserDTO userDto)
        {
            if (_context.Users.Any(u => u.UserName == userDto.UserName))
                return (false, "Username already exists.");

            if (_context.Users.Any(u => u.Email == userDto.Email))
                return (false, "Email already exists.");

            // Проверяем, существует ли роль
            if (!await _context.Roles.AnyAsync(r => r.Id == userDto.RoleId))
                return (false, "Selected role does not exist.");

            // Маппинг только основных данных
            var user = _mapper.Map<User>(userDto);
            user.PasswordHash = SecurePasswordHasher.Hash(userDto.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return (true, "User created successfully.");
        }



        /// <summary>
        /// Обновление данных пользователя.
        /// </summary>
        public async Task<(bool success, string errorMessage)> UpdateUserAsync(UserDTO userDto)
        {
            var user = await _context.Users.FindAsync(userDto.Id);
            if (user == null)
                return (false, "User not found.");

            if (_context.Users.Any(u => u.UserName == userDto.UserName && u.Id != userDto.Id))
                return (false, "Username already exists.");

            if (_context.Users.Any(u => u.Email == userDto.Email && u.Id != userDto.Id))
                return (false, "Email already exists.");

            user = _mapper.Map(userDto, user);

            if (!SecurePasswordHasher.Verify(userDto.Password, user.PasswordHash))
            {
                user.PasswordHash = SecurePasswordHasher.Hash(userDto.Password);
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return (true, "User updated successfully.");
        }

        /// <summary>
        /// Удаление пользователя по ID.
        /// </summary>
        public async Task<(bool success, string errorMessage)> DeleteUserAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return (false, "User not found.");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return (true, "User deleted successfully.");
        }

        /// <summary>
        /// Проверка существования пользователя по ID.
        /// </summary>
        public bool UserExists(Guid id)
        {
            return _context.Users.Any(u => u.Id == id);
        }

       
    }
}