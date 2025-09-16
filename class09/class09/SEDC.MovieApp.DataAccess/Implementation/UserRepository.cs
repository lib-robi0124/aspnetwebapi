using Microsoft.EntityFrameworkCore;
using SEDC.MovieApp.Domain.Domain;

namespace SEDC.MovieApp.DataAccess.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly MoviesDbContext _context;
        public UserRepository(MoviesDbContext context)
        {
            _context = context;
        }
        public void Add(User entity)
        {
            _context.Users.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(User entity)
        {
            _context.Users.Remove(entity);
            _context.SaveChanges();
        }

        public List<User> GetAll()
        {
            return _context.Users.Include(x => x.MovieList).ToList();
        }

        public User GetById(int id)
        {
            return _context.Users.Include(x => x.MovieList).FirstOrDefault(x => x.Id == id);
        }

        public User GetUserByUsername(string username)
        {
            return _context.Users.FirstOrDefault(x => x.Username.ToLower() == username.ToLower());
        }

        public User LoginUser(string username, string hashedPassword)
        {
            return _context.Users.FirstOrDefault(x => x.Username.ToLower() == username.ToLower() && x.Password == hashedPassword);
        }

        public void Update(User entity)
        {
            _context.Users.Update(entity);
            _context.SaveChanges();
        }
    }
}
