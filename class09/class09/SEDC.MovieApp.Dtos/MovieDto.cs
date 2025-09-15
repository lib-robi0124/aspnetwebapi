using SEDC.MovieApp.Domain.Enums;

namespace SEDC.MovieApp.Dtos
{
    public class MovieDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Year { get; set; }
        public GenreEnum Genre { get; set; }
        public int UserId { get; set; }
    }
}
