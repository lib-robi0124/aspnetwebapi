using Avenga.NotesApp.DataAccess.Interfaces;
using Avenga.NotesApp.Domain.Models;
using Avenga.NotesApp.Dtos.UserDtos;
using Avenga.NotesApp.Services.Interfaces;
using Avenga.NotesApp.Shared.CustomExceptions;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using XSystem.Security.Cryptography;

namespace Avenga.NotesApp.Services.Implementations
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public string LoginUser(LoginUserDto loginUserDto)
        {
            //validations
            if (string.IsNullOrEmpty(loginUserDto.Username) || string.IsNullOrEmpty(loginUserDto.Password))
            {
                throw new UserDataException("Username and password are required.");
            }
            //hash password
            MD5CryptoServiceProvider md5CryptoServiceProvider = new MD5CryptoServiceProvider();
            byte[] passwordBytes = Encoding.ASCII.GetBytes(loginUserDto.Password);
            byte[] hashedBytes = md5CryptoServiceProvider.ComputeHash(passwordBytes);
            string hashedPassword = Encoding.ASCII.GetString(hashedBytes);
            //check user
            User userDb = _userRepository.LoginUser(loginUserDto.Username, hashedPassword);
            if (userDb == null)
            {
                throw new UserDataException("Invalid username or password.");
            }
            //generate token JWT
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            byte[] secretKeyBytes = Encoding.ASCII.GetBytes("Our very very secret secret key!");
            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
            {
                //payload-claims
                Subject = new System.Security.Claims.ClaimsIdentity(new[]
                {
                    new Claim("Id", userDb.Id.ToString()),
                    new Claim(ClaimTypes.Name, userDb.Username),
                    new Claim("FirstName", userDb.FirstName ?? ""),
                    new Claim("LastName", userDb.LastName ?? ""),
                    new Claim(ClaimTypes.Role, "User"),
                    new Claim("userFullName", $"{userDb.FirstName} {userDb.LastName}")
                }),
                Expires = DateTime.UtcNow.AddHours(1), //token valid for 1 hour upon generation
                //signing credentials-configuration
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha256Signature)
            };
            //create token
            SecurityToken securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            return jwtSecurityTokenHandler.WriteToken(securityToken);
            
         }

        public void RegisterUser(RegisterUserDto registerUserDto)
        {
            //validations
            ValidateUser(registerUserDto);

            //2. hash password MD5 hashing algorithm
            MD5CryptoServiceProvider md5CryptoServiceProvider = new MD5CryptoServiceProvider();
            byte[] passwordBytes = Encoding.ASCII.GetBytes(registerUserDto.Password);
            byte[] hashedBytes = md5CryptoServiceProvider.ComputeHash(passwordBytes);
            string hashedPassword = Encoding.ASCII.GetString(hashedBytes);
            //3. create user
            User user = new User
            {
                FirstName = registerUserDto.FirstName,
                LastName = registerUserDto.LastName,
                Username = registerUserDto.Username,
                Password = hashedPassword
            };
            //4. save user
            _userRepository.Add(user);
        }

        private void ValidateUser(RegisterUserDto registerUserDto)
        {
            //validations
            if (string.IsNullOrEmpty(registerUserDto.Username) || registerUserDto.Username.Length > 30)
            {
                throw new UserDataException("Username is required and should be less than 30 characters.");
            }
            if (registerUserDto.Username.Length > 30)
            {
                throw new UserDataException("Username should be less than 30 characters.");
            }
            if (!string.IsNullOrEmpty(registerUserDto.FirstName) && registerUserDto.FirstName.Length > 50)
            {
                throw new UserDataException("First name should be less than 50 characters.");
            }
            if (!string.IsNullOrEmpty(registerUserDto.LastName) && registerUserDto.LastName.Length > 50)
            {
                throw new UserDataException("Last name should be less than 50 characters.");
            }
            if (registerUserDto.Password != registerUserDto.ConfirmPassword)
            {
                throw new UserDataException("Password not much!");
            }
            var userDb = _userRepository.GetUserByUsername(registerUserDto.Username);
            if (userDb != null)
            {
                throw new UserDataException("Username already exists.");
            }
        }
    }
}
