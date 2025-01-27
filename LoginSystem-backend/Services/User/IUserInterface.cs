using LoginSystem.Dtos;
using LoginSystem.Models;
using Microsoft.OpenApi.Any;

namespace LoginSystem.Services.User
{
    public interface IUserInterface
    {
        Task<ResponseModel<List<UserModel>>> RegisterUser(RegisterUserDto registerUserDto);
        Task<ResponseModel<UserModel>> Login(LoginDto loginDto);
        Task<ResponseModel<AnyType>> Logout();
        Task<ResponseModel<UserModel>> GetUser();
    }
}
