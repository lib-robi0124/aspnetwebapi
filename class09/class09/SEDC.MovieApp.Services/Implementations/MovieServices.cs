using SEDC.MovieApp.DataAccess;
using SEDC.MovieApp.Domain.Domain;
using SEDC.MovieApp.Domain.Enums;
using SEDC.MovieApp.Dtos;
using SEDC.MovieApp.Mappers;
using SEDC.MovieApp.Services.Interfaces;
using SEDC.MovieApp.Shared;

namespace SEDC.MovieApp.Services.Implementations
{
    public class MovieServices : IMovieService
    {
        private readonly IMovieRepository _movieRepository;
        public MovieServices(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public void AddMovie(AddMovieDto addMovieDto)
        {
            if (string.IsNullOrEmpty(addMovieDto.Title))
            {
                throw new MovieException("Title is required");
            }
            if (addMovieDto.Year < 1900 || addMovieDto.Year > DateTime.Now.Year)
            {
                throw new MovieException("Year is not valid");
            }
            if (string.IsNullOrEmpty(addMovieDto.Description) && addMovieDto.Description.Length > 250)
            {
                throw new MovieException("Description is required and must be less than 250 characters");
            }
            Movie newMovie = addMovieDto.ToMovie();
            _movieRepository.Add(newMovie);
        }

        public void DeleteMovie(int id)
        {
            var movieDb = _movieRepository.GetById(id);
            if (movieDb == null)
            {
                throw new MovieNotFoundException($"Movie with {id} not found");
            }
            _movieRepository.Delete(movieDb);
        }

        public List<MovieDto> FilterMovies(int? year, GenreEnum? genre)
        {
            throw new NotImplementedException();
        }

        public List<MovieDto> GetAllMovies()
        {
            throw new NotImplementedException();
        }

        public MovieDto GetMovieById(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdateMovie(UpdateMovieDto updateMovieDto)
        {
            throw new NotImplementedException();
        }
    }
}
