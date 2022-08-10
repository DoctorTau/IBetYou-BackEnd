using IBUAPI.Models;
using IBUAPI.Models.Dto;

namespace IBUAPI.Services;
public interface IUserRegistration
{
    Task<User> RegisterUser(UserRegistrationDto userToReg);
    string Login(UserLoginDto user);
}

