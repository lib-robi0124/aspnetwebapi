using SEDC.MovieApp.Domain.Domain;
using SEDC.MovieApp.Dtos;

namespace SEDC.MovieApp.Mappers
{
    public static class MovieMapper
    {
        // Mapping methods would go here
        public static Movie ToMovie(this AddMovieDto addMovieDto)
        {
            return new Movie
            {
                Title = addMovieDto.Title,
                Description = addMovieDto.Description,
                Year = addMovieDto.Year,
                Genre = addMovieDto.Genre
            };
        }
        public static MovieDto ToMovieDto(this Movie movie)
        {
            return new MovieDto
            {
                Title = movie.Title,
                Description = movie.Description,
                Year = movie.Year,
                Genre = movie.Genre
            };
        }
    }
}
