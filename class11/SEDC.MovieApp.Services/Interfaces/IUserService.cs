using SEDC.MovieApp.Dtos.UserDto;

namespace SEDC.MovieApp.Services.Interfaces
{
    public interface IUserService
    {
        void RegisterUser(RegisterUserDto registerUserDto);
        string LoginUser(LoginUserDto loginUserDto);
        List<UserDto> GetAllUsers();
        UserDto GetUserById(int id);
        void UpdateUser(UpdateUserDto updateUserDto);
        void DeleteUser(int id);
    }
}
