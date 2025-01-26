﻿using LoginSystem.Dtos;
using LoginSystem.Models;
using LoginSystem.Services.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LoginSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AuthController : Controller
    {
        private readonly IUserInterface _userInterface;

        public AuthController(IUserInterface userInterface)
        {
            _userInterface = userInterface;
        }

        [HttpPost("registerUser")]
        public async Task<ActionResult<ResponseModel<List<UserModel>>>> RegisterUser(RegisterUserDto registerUserDto)
        {
            var user = await _userInterface.RegisterUser(registerUserDto);
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<ResponseModel<List<UserModel>>>> Login(LoginDto loginDto)
        {
            var user = await _userInterface.Login(loginDto);
            return Ok(user);
        }
    }
}
