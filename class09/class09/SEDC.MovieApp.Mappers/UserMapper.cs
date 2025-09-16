using SEDC.MovieApp.Domain.Domain;
using SEDC.MovieApp.Dtos.UserDto;

namespace SEDC.MovieApp.Mappers
{
    public static class UserMapper
    {
        public static UserDto ToUserDto(this User user)
        {
            if (user == null) return null;
            return new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.Username,
            };
        }
        public static User ToUser(this UserDto userDto)
        {
            if (userDto == null) return null;
            return new User
            {
                Id = userDto.Id,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Username = userDto.Username
            };
        }

        //public static User ToUser(this UpdateUserDto updateUserDto, User existingUser)
        //{
        //    existingUser.FirstName = updateUserDto.FirstName;
        //    existingUser.LastName = updateUserDto.LastName;
        //    existingUser.Username = updateUserDto.Username;
        //    return existingUser;
        //}
    }
}
