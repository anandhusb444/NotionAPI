using Microsoft.EntityFrameworkCore;
using NotionAPI.Context;
using NotionAPI.Models;

namespace NotionAPI.Services
{
    public interface IuserService
    {
        public Task<bool> AddUser(Users user);
    }
    public class UserService : IuserService
    {
        public NotionData _context;

        public UserService(NotionData context)
        {
            _context = context;
        }
        public async Task<bool> AddUser(Users user)
        {
            //check the user already exist in the databse

            Users currentUser = await _context.users.Where(u => u.Email == user.Email).FirstOrDefaultAsync();

            if(currentUser != null)
            {
                return false;
            }

            await _context.users.AddAsync(user);
            await _context.SaveChangesAsync();

            return true;
        }
        
    }
}
