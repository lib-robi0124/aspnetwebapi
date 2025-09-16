using SEDC.MovieApp.Domain.Domain;
using SEDC.MovieApp.Domain.Enums;

namespace SEDC.MovieApp.DataAccess.Implementation
{
    public class MovieRepository : IMovieRepository
    {
        private readonly MoviesDbContext _context;
        public MovieRepository(MoviesDbContext context)
        {
            _context = context;
        }
        public void Add(Movie entity)
        {
            _context.Movies.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(Movie entity)
        {
            _context.Movies.Remove(entity);
            _context.SaveChanges();
        }

        public IEnumerable<Movie> FilterMovies(int? year, GenreEnum? genre)
        {
            if (genre == null && year == null)
            {
                return _context.Movies.ToList();
            }
            if (year == null)
            {
                List<Movie> moviesDb = _context.Movies.Where(x => x.Genre == (GenreEnum)genre).ToList();
                return moviesDb;
            }
            if (genre == null)
            {
                return _context.Movies.Where(x => x.Year == year).ToList();
            }
            return _context.Movies.Where(x => x.Year == year && x.Genre == genre).ToList();
            // or  List<Movie> movies = _context.Movies.Where(x => x.Year == year && x.Genre == (GenreEnum)genre).ToList();  return movies;
        }

        public List<Movie> GetAll()
        {
            return _context.Movies.ToList();
        }

        public Movie GetById(int id)
        {
            return _context.Movies.SingleOrDefault(x => x.Id == id);
        }

        public void Update(Movie entity)
        {
            _context.Movies.Update(entity);
            _context.SaveChanges();
        }

    }
}
