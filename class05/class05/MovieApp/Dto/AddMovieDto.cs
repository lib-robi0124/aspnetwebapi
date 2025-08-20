using MovieApp.Models;

namespace MovieApp.Dto
{
    public class AddMovieDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Year { get; set; }
        public GenreEnum Genre { get; set; }
    }
}
