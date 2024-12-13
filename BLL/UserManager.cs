// UserManager.cs in BLL namespace

using BLL.DTO;
using WebAuthCoreBLL.Helpers;
using BLL.Services;
using DAL.Models;
using Microsoft.EntityFrameworkCore;


namespace BLL
{
    public class UserManager
    {
        private readonly CrmContext _context;

        public UserManager(CrmContext context)
        {
            _context = context;
        }

        public CrmContext GetContext()
        {
            return _context;
        }

        public async Task<IEnumerable<User>> GetAllUsersWithRoles()
        {
            return await _context.Users.Include(u => u.Role).ToListAsync();
        }

        public async Task<bool> DeleteUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                // User not found
                return false;
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        // Business logic for checking if a user exists
        public bool UserExists(Guid id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        // Business logic for getting a user by ID
        public async Task<User> GetUserById(Guid id)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.Id == id);
            var img = await _context.MediaData
                     .Where(m => m.AssociatedRecordId == user.Id)
                     .FirstOrDefaultAsync();
            return user; // This could be null if user doesn't exist
        }

        // For example, updating user password or roles, etc.
        public async Task<bool> UpdatePassword(int userId, string newPassword)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                user.PasswordHash = SecurePasswordHasher.Hash(newPassword);
                _context.Update(user);
                await _context.SaveChangesAsync();
                return true;
            }
            return false; // User not found or other error
        }

        // Business logic for editing a user
        public async Task<(bool success, string errorMessage)> EditUser(Guid id, UserDTO model)
        {
            var user = new User()
            {
                Id = model.Id,
                UserName = model.UserName,
                Password = model.Password,
                Email = model.Email,
                Phone = model.Phone,
                FirstName = model.FirstName,
                LastName = model.LastName,
                IsActive = model.IsActive,
                RoleId = model.RoleId
            };

            if (id != user.Id)
            {
                // Handle user not found or id mismatch scenario
                return (false, "");
            }
            if (_context.Users.Any(u => u.UserName == user.UserName && u.Id != user.Id))
            {
                // Handle username already exists scenario
                return (false, "Username already exists.");
            }

            if (_context.Users.Any(u => u.Email == user.Email && u.Id != user.Id))
            {
                // Handle email already exists scenario
                return (false, "Email already exists.");
            }
            if (_context.Users.Any(u => u.Phone == user.Phone && u.Id != user.Id))
            {
                // Handle email already exists scenario
                return (false, "Mobile number already exists.");
            }

            try
            {
                // Update logic here
                // Hashing the password and other pre-save logic
               
                if (_context.Users.Any(u => u.PasswordHash != user.Password && u.Id == user.Id))
                {
                    user.PasswordHash = SecurePasswordHasher.Hash(user.Password);
                }
                else user.PasswordHash = model.Password;
                _context.Update(user);
                await _context.SaveChangesAsync();

                var mediaService = new MediaService(_context);
                await mediaService.SetAvatarAsync(model, user.Id, "Image");

                await _context.SaveChangesAsync();

                return (true, "");
            }
            catch (DbUpdateConcurrencyException)
            {
                // Handle concurrency issues or user not found scenario
                return (false, "");
            }
        }

        public async Task<(bool success, string errorMessage)> CreateUser(UserDTO model)
        {
            var user = new User()
            {
                UserName = model.UserName,
                Password = model.Password,
                Email = model.Email,
                Phone = model.Phone,
                FirstName = model.FirstName,
                LastName = model.LastName,
                IsActive = model.IsActive,
                RoleId = model.RoleId,
                ResetPasswordToken=model.ResetPasswordToken
            };

            if (_context.Users.Any(u => u.UserName == user.UserName))
            {
                // Handle username already exists scenario
                return (false, "Username already exists.");
            }

            if (_context.Users.Any(u => u.Email == user.Email))
            {
                // Handle email already exists scenario
                return (false, "Email already exists.");
            }
            if (_context.Users.Any(u => u.Phone == user.Phone))
            {
                // Handle email already exists scenario
                return (false, "Mobile number already exists.");
            }

            // Hashing the password and other pre-save logic
            user.PasswordHash = SecurePasswordHasher.Hash(user.Password);
            _context.Add(user);

            await _context.SaveChangesAsync();

            //if (model.ImageFile != null)
            //{
            //    using (var stream = new MemoryStream())
            //    {
            //        await model.ImageFile.CopyToAsync(stream);
            //        var imageData = stream.ToArray();

            //        // Create MediaData object for the image
            //        var mediaData = new MediaDatum
            //        {
            //            Id = Guid.NewGuid(), // Generate a new Id for the media data
            //            Name = model.ImageFile.FileName,
            //            Extension = Path.GetExtension(model.ImageFile.FileName),
            //            Content = imageData,
            //            AssociatedRecordId = user.Id, // Associate with the Use
            //            IsPrime = true, // Set as non-primary for additional images
            //            ObjectTypeId = 1 // Assuming Image is an enumerated type representing media types
            //        };

            //        // Save media data to the database
            //        await _context.MediaData.AddAsync(mediaData);
            //        await _context.SaveChangesAsync();
            //    }
            //}
            if (!string.IsNullOrEmpty(model.Base64Image) && !string.IsNullOrEmpty(model.ImgName))
            {
                // Convert base64 string to byte array
                byte[] imageData = Convert.FromBase64String(model.Base64Image);
                var mediaData = new MediaDatum
                {
                    Id = Guid.NewGuid(), // Generate a new Id for the media data
                    Name = model.ImgName,
                    Extension = Path.GetExtension(model.ImgName),
                    Content = imageData,
                    AssociatedRecordId = user.Id, // Associate with the User
                    IsPrime = true, // Set as primary for the user
                    ObjectTypeId = 1 // Assuming Image is an enumerated type representing media types
                };

                // Add new media data to the context
                _context.MediaData.Add(mediaData);
                await _context.SaveChangesAsync();
            }

            return (true, "");
        }


        public async Task<User> ValidateUser(string login, string password)
        {
            var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.UserName == login);
            if (user != null && SecurePasswordHasher.Verify(password, user.PasswordHash))
            {
                return user; // Valid user
            }
            return null; // Invalid user or credentials
        }



    }
}
