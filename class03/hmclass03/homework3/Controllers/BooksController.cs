using homework3.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace homework3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        [HttpGet] // GET: api/books /rfc 5
        public ActionResult<List<Book>> GetBooks()
        {
            try
            {
                // Return the list of books from StaticDb
                return Ok(StaticDb.Books);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occured! Contact the admin {e.Message}");
            }

        }
        [HttpGet("queryString")] // GET: api/books/queryString?index=0 /rfc 6
        public ActionResult<Book> GetBookByIndex([FromQuery] int? index)
        {
            try
            {
                // Check if the index is null or invalid
                if (!index.HasValue || index < 0 || index >= StaticDb.Books.Count)
                {
                    return NotFound($"There is no resourse on index {index}, index negative or missing");
                }
                // Return the book at the specified index
                return Ok(StaticDb.Books[index.Value]);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occured! Contact the admin {e.Message}");
            }
        }
        [HttpGet("filter")] // GET: api/books/filter?author=George%20Orwell&title=1984 /rfc 7
        public ActionResult<List<Book>> GetBooksByFilter([FromQuery] string? author, [FromQuery] string? title)
        {
            try
            {
                if (string.IsNullOrEmpty(author) && string.IsNullOrEmpty(title))
                {
                    return BadRequest("At least one filter parameter (author or title) must be provided.");
                }
                // Filter books by author and title
                List<Book> filteredBooks = StaticDb.Books
                    .Where(b =>
                        (string.IsNullOrEmpty(author) || b.Author.ToLower().Contains(author.ToLower())) &&
                        (string.IsNullOrEmpty(title) || b.Title.ToLower().Contains(title.ToLower()))
                    )
                    .ToList();
                // Check if any books match the filter
                if (filteredBooks.Count == 0)
                {
                    return NotFound("No books found matching the specified criteria.");
                }
                return Ok(filteredBooks);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occured! Contact the admin {e.Message}");
            }
        }
        [HttpPost] // POST: api/books /rfc 8
        public IActionResult AddBook([FromBody] Book book)
        {
            try
            {
                // Validate the book object
                if (book == null || string.IsNullOrEmpty(book.Title) || string.IsNullOrEmpty(book.Author))
                {
                    return BadRequest("Invalid book data. Title and Author are required.");
                }
                // Add the new book to the StaticDb
                StaticDb.Books.Add(book);
                return CreatedAtAction(nameof(GetBooks), new { title = book.Title }, book);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occured! Contact the admin {e.Message}");
            }
        }
        [HttpPost("list")] // POST: api/books/list //bonus
        public IActionResult List()
        {
            try
            {
                // Get the list of books from the request body
                List<Book> books = StaticDb.Books;
                // Check if the list is empty
                if (books == null || books.Count == 0)
                {
                    return NotFound("No books found.");
                }
                // Return the titles of the books as a list of strings
                var titles = books.Select(b => b.Title).ToList();
                return Ok(titles);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occured! Contact the admin {e.Message}");
            }
        }
    }
}
