using Azure;
using BCrypt.Net;
using LoginSystem.Data;
using LoginSystem.Dtos;
using LoginSystem.Helpers;
using LoginSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;

namespace LoginSystem.Services.User
{
    public class UserService : IUserInterface
    {
        public readonly AppDbContext _context;
        public readonly JwtService _jwtService;
        public readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(AppDbContext context, JwtService jwtService, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _jwtService = jwtService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseModel<List<UserModel>>> RegisterUser(RegisterUserDto dto)
        {
            ResponseModel<List<UserModel>> response = new ResponseModel<List<UserModel>>();

            try
            {
                var user = new UserModel
                {
                    Name = dto.Name,
                    Email = dto.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                    Phone = dto.Phone,
                };

                _context.Add(user);
                await _context.SaveChangesAsync();

                response.Data = await _context.Users.ToListAsync();
                response.Message = "User registred!";
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<UserModel>> Login(LoginDto dto)
        {
            ResponseModel<UserModel> response = new ResponseModel<UserModel>();

            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(user => user.Email == dto.Email);

                if (user == null)
                {
                    response.Message = "Email not found!";
                    return response;
                }

                if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
                {
                    response.Message = "Wrong password!";
                    return response;
                }

                var jwt = _jwtService.Generate(user.Id);
                _httpContextAccessor.HttpContext.Response.Cookies.Append("jwt", jwt, new CookieOptions { HttpOnly = true });

                response.Data = user;
                response.Message = "Logged in!";
                return response;
            } 
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<AnyType>> Logout()
        {
            ResponseModel<AnyType> response = new ResponseModel<AnyType>();

            _httpContextAccessor.HttpContext.Response.Cookies.Delete("jwt");
            response.Message = "Logged out!";
            return response;
        }

        public async Task<ResponseModel<UserModel>> GetUser()
        {
            ResponseModel<UserModel> response = new ResponseModel<UserModel>();

            try
            {
                var jwt = _httpContextAccessor.HttpContext.Request.Cookies["jwt"];
                var token = _jwtService.Verify(jwt);
                int userId = int.Parse(token.Issuer);
                var user = await _context.Users.FirstOrDefaultAsync(user => user.Id == userId);

                response.Data = user;
                response.Message = "User getted";
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

    }
}
