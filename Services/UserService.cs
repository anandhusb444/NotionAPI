using Microsoft.EntityFrameworkCore;
using NotionAPI.Context;
using NotionAPI.DTOs.User;
using NotionAPI.Models;
using NotionAPI.Utilites;

namespace NotionAPI.Services
{
    public interface IuserService
    {
        public Task<GenericRespones<Users>> AddUser(UserDto user);
        public Task<bool> LoginUser(int id);
    }
    public class UserService : IuserService
    {
        public NotionData _context;

        public UserService(NotionData context)
        {
            _context = context;
        }
        public async Task<GenericRespones<Users>> AddUser(UserDto user)
        {
            try
            {
                Users currentUser = await _context.users.Where(u => u.Email == user.email).FirstOrDefaultAsync();

                if (currentUser != null)
                {
                    return new GenericRespones<Users>("User already exist","Cannot create user already exist in DB",400,null,false);
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

                return new GenericRespones<Users>("Successfuly added user",null,200,newUser,true);
            }
            catch (Exception ex)
            {

                return new GenericRespones<Users>("Exception error",ex.Message,500,null,false);
            }
        }

        public async Task<bool> LoginUser(int id)
        {
            try
            {
                Users user = await _context.users.FindAsync(id);

                if(user == null)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        
    }
}
