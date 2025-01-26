﻿using BCrypt.Net;
using LoginSystem.Data;
using LoginSystem.Dtos;
using LoginSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LoginSystem.Services.User
{
    public class UserService : IUserInterface
    {
        public readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
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
    }
}
