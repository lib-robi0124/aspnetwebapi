using Avenga.NotesApp.Domain.Models;
using Avenga.NotesApp.Dtos.UserDtos;

namespace Avenga.NotesApp.Mappers
{
    public static class UserMapper
    {
        // the this keyword will extend the user class so we can use ToUserDto()
        //on any object from type User
        public static UserDto ToUserDto(this User user)
        {
            if (user == null) return null;
            return new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.Username
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
    }
}
