using SEDC.MovieApp.Domain.Enums;
using SEDC.MovieApp.Dtos;

namespace SEDC.MovieApp.Services.Interfaces
{
    public interface IMovieService
    {
        List<MovieDto> GetAllMovies();
        List<MovieDto> FilterMovies(int? year, GenreEnum? genre);
        MovieDto GetMovieById(int id);
        void AddMovie(AddMovieDto addMovieDto);
        void UpdateMovie(UpdateMovieDto updateMovieDto);
        void DeleteMovie(int id);
    }
}
