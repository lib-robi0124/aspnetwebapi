using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieApp.Dto;

namespace MovieApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<MovieDto>> GetMovies()
        {
            try
            {
                var moviesDb = StaticDb.Movies;
                return Ok(moviesDb.Select(m => new MovieDto
                {
                    Title = m.Title,
                    Description = m.Description,
                    Year = m.Year,
                    Genre = m.Genre
                }).ToList());
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error occured");

            }
        }
        [HttpGet("{id}")]
        public ActionResult<MovieDto> GetMovie(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "Bad request, the id can not be negative!");
                }
                var movie = StaticDb.Movies.FirstOrDefault(m => m.Id == id);
                if (movie == null)
                {
                    return NotFound("Movie not found");
                }
                return Ok(new MovieDto
                {
                    Title = movie.Title,
                    Description = movie.Description,
                    Year = movie.Year,
                    Genre = movie.Genre
                });
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error occured");
            }
        }
    }
}
