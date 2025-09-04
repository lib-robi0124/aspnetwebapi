using SEDC.MovieApp.Domain.Domain;

namespace SEDC.MovieApp.DataAccess
{
    public interface IUserRepository : IRepository<User>
    {
        User LoginUser(string username, string hashedPassword);
        User GetUserByUsername(string username);


    }
}
