using SEDC.MovieApp.Dtos.UserDto;

namespace SEDC.MovieApp.Services.Interfaces
{
    public interface IUserService
    {
        void RegisterUser(RegisterUserDto registerUserDto);
        string LoginUser(LoginUserDto loginUserDto);
    }
}
