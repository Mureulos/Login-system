using LoginSystem.Dtos;
using LoginSystem.Models;

namespace LoginSystem.Services.User
{
    public interface IUserInterface
    {
        Task<ResponseModel<List<UserModel>>> RegisterUser(RegisterUserDto registerUserDto);
        Task<ResponseModel<UserModel>> Login(LoginDto loginDto);
        Task<ResponseModel<UserModel>> GetUser();
    }
}
