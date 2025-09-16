using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SEDC.MovieApp.Domain.Enums;
using SEDC.MovieApp.Dtos;
using SEDC.MovieApp.Services.Interfaces;
using SEDC.MovieApp.Shared;

namespace SEDC.MovieApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        // Controller code will go here
        private readonly IMovieService _movieService;
        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }
        // Add endpoints here
        [HttpGet]
        public ActionResult<List<MovieDto>> Get()
        {
            try
            {
                return Ok(_movieService.GetAllMovies());
            }
            catch (MovieException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred, contact the admin");
            }
        }
        [HttpGet("{id}")]
        public ActionResult<MovieDto> Get(int id)
        {
            try
            {
                return Ok(_movieService.GetMovieById(id));
            }
            catch (MovieNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (MovieException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred, contact the admin");
            }
        }
        [HttpGet("filter")]
        public ActionResult<List<MovieDto>> Filter(int year, GenreEnum? genre)
        {
            try
            {
                return Ok(_movieService.FilterMovies(year, genre));
            }
            catch (MovieException e)
            {
                //log
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                //log
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred, contact the admin");
            }
        }
        [HttpPut]
        public ActionResult Update([FromBody] UpdateMovieDto updateMovieDto)
        {
            try
            {
                _movieService.UpdateMovie(updateMovieDto);
                return Ok();
            }
            catch (MovieNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (MovieException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred, contact the admin");
            }
        }
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                _movieService.DeleteMovie(id);
                return StatusCode(StatusCodes.Status204NoContent, "Deleted resource");
            }
            catch (MovieNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (MovieException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred, contact the admin");
            }
        }
        [HttpPost("addMovie")]
        public ActionResult Add([FromBody] AddMovieDto addMovieDto)
        {
            try
            {
                _movieService.AddMovie(addMovieDto);
                return StatusCode(StatusCodes.Status201Created, "Resource created");
            }
            catch (MovieException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred, contact the admin");
            }
        }
    }
}
