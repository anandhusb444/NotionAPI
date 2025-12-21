using Microsoft.EntityFrameworkCore;
using NotionAPI.Context;
using NotionAPI.DTOs.User;
using NotionAPI.Models;
using NotionAPI.Utilites;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;


namespace NotionAPI.Services
{
    public interface IuserService
    {
        public Task<GenericRespones<Users>> AddUser(UserRegister user);
        public Task<GenericRespones<string>> LoginUser(UserLogin user);
    }
    public class UserService : IuserService
    {
        public NotionData _context;
        public IConfiguration _config;

        public UserService(NotionData context,IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        public async Task<GenericRespones<Users>> AddUser(UserRegister user)
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

        public async Task<GenericRespones<string>> LoginUser(UserLogin user)
        {
            try
            {
                Users userLogin = await _context.users.FirstOrDefaultAsync(u => u.Email == user.email && u.Password == user.password);

                if(userLogin == null)
                {
                    return new GenericRespones<string>("User not found","Not found",403,null,false);
                }

                var claim = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userLogin.Id.ToString()),
                    new Claim(ClaimTypes.Name,userLogin.Email),
                    new Claim(ClaimTypes.Role,"user")
                };

                var key = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

                var creadentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claim,
                expires: DateTime.UtcNow.AddMinutes(
                Convert.ToDouble(_config["Jwt:ExpiryInMinutes"])
                ),
                    signingCredentials: creadentials);

                var genToken = new JwtSecurityTokenHandler().WriteToken(token);

                return new GenericRespones<string>("User login successful","",200,genToken,true);
            }
            catch (Exception ex)
            {
                return new GenericRespones<string>("Internal server error",ex.Message,500,null,false);
            }
        }
        
    }
}
