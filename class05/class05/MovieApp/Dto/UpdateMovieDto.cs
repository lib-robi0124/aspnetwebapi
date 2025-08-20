using MovieApp.Models;

namespace MovieApp.Dto
{
    public class UpdateMovieDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Year { get; set; }
        public GenreEnum Genre { get; set; }
    }
}
