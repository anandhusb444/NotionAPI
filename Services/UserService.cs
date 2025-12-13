using Microsoft.EntityFrameworkCore;
using NotionAPI.Context;
using NotionAPI.DTOs.User;
using NotionAPI.Models;

namespace NotionAPI.Services
{
    public interface IuserService
    {
        public Task<bool> AddUser(UserDto user);
    }
    public class UserService : IuserService
    {
        public NotionData _context;

        public UserService(NotionData context)
        {
            _context = context;
        }
        public async Task<bool> AddUser(UserDto user)
        {
            try
            {
                Users currentUser = await _context.users.Where(u => u.Email == user.email).FirstOrDefaultAsync();

                if (currentUser != null)
                {
                    return false;
                }

                Users newUser = new Users()
                {
                    Name = user.name,
                    Email = user.email,
                    Password = user.password,
                    CreatedAt = DateTime.UtcNow,
                };


                await _context.users.AddAsync(newUser);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
            
        }
        
    }
}
