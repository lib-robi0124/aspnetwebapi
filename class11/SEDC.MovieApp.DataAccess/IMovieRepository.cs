using SEDC.MovieApp.Domain.Domain;
using SEDC.MovieApp.Domain.Enums;

namespace SEDC.MovieApp.DataAccess
{
    public interface IMovieRepository : IRepository<Movie>
    {
        IEnumerable<Movie> FilterMovies(int? year, GenreEnum? genre);
    }
}
