using Microsoft.AspNetCore.Mvc;
using NotesAndTagssApp.Models;

namespace NotesAndTagssApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        [HttpGet] //http://localhost:[port]/api/notes
        public ActionResult<List<Note>> Get()
        {
            try
            {
                return Ok(StaticDb.Notes);
            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, $"an error occured! Contact the admin!");
            }
        }
        [HttpGet("{index}")] //http://localhost:[port]/api/notes/{index}
        public ActionResult<Note> Get(int index)
        {
            try
            {
                if (index < 0)
                {
                    return BadRequest("Index cannot be negative.");
                }
                if (index >= StaticDb.Notes.Count)
                {
                    return NotFound($"Note with index {index} not found.");
                }
                return Ok(StaticDb.Notes[index]);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"an error occured! Contact the admin!");
            }
        }
        [HttpGet("queryString")] //http:localhost:[port]/api/notes/queryString?index=1
        public ActionResult<Note> GetQueryString(int? index)
        {
            try
            {
                if (index == null)
                {
                    return BadRequest("Index is a required parameter");
                }
                if (index < 0)
                {
                    return BadRequest("Index cannot be negative.");
                }
                if (index >= StaticDb.Notes.Count)
                {
                    return NotFound($"Note with index {index} not found.");
                }
                return Ok(StaticDb.Notes[index.Value]);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"an error occured! Contact the admin!");
            }
        }
        [HttpGet("{text}/priority/{priority}")] //http://localhost:[port]/api/notes/{gym}/priority/{1}
        public ActionResult<List<Note>> GetByTextAndPriority(string text, int priority)
        {
            try
            {
                if (string.IsNullOrEmpty(text) || priority <= 0)
                {
                    return BadRequest("Text and priority are required parameters");
                }
                if (priority > 3)
                {
                    return BadRequest("Priority must be between 1 and 3.");
                }
                List<Note> notesDb = StaticDb.Notes
                    .Where(n => n.Text.ToLower().Contains(text.ToLower()) && (int)n.Priority == priority)
                    .ToList();
                return Ok(notesDb);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"an error occured! Contact the admin!");
            }
        }
        [HttpGet("multipleParams")] //http://localhost:[port]/api/notes/multipleParams?text=gym&priority=1
        public ActionResult<List<Note>> FilterNotesByMultipleParams(string? text, int? priority)
        {
            try
            {
                if (string.IsNullOrEmpty(text) && priority == null)
                {
                    return BadRequest("Text or priority are required parameters");
                }
                if (priority > 3)
                {
                    return BadRequest("Priority must be between 1 and 3.");
                }
                if (priority == null)
                {
                    priority = 1; // Default priority if not provided
                    List<Note> filteredNotes = StaticDb.Notes
                        .Where(x => x.Text.ToLower().Contains(text.ToLower()))
                        .ToList();
                    return Ok(filteredNotes);
                }
                if (string.IsNullOrEmpty(text))
                {
                    text = string.Empty; // Default text if not provided
                    List<Note> filteredNotes = StaticDb.Notes
                        .Where(x => (int)x.Priority == priority)
                        .ToList();
                }
                List<Note> notesDb = StaticDb.Notes
                    .Where(n => n.Text.ToLower().Contains(text.ToLower()) && (int)n.Priority == priority)
                    .ToList();
                return Ok(notesDb);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"an error occured! Contact the admin!");
            }
        }
        [HttpGet("header")]
        public IActionResult GetHeader([FromHeader(Name = "TestHeader")] string testHeader)
        {
            return Ok(testHeader);
        }

        [HttpGet("userAgent")]
        public IActionResult GetUserAgentHeader([FromHeader(Name = "User-Agent")] string userAgent)
        {
            return Ok(userAgent);
        }
        [HttpPost]
        public IActionResult PostNote([FromBody] Note note)
        {
            try
            {
                if (note == null)
                {
                    return BadRequest("Note cannot be null.");
                }
                if (string.IsNullOrEmpty(note.Text))
                {
                    return BadRequest("Text is a required field.");
                }
               if (note.Tags == null || note.Tags.Count == 0)
                {
                    return BadRequest("At least one tag is required.");
                }
                StaticDb.Notes.Add(note);
                return CreatedAtAction(nameof(Get), new { index = StaticDb.Notes.Count - 1 }, note);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"an error occured! Contact the admin!");
            }
        }
        [HttpPut("updateNote/{index}")]
        public IActionResult UpdateNote(int index, [FromBody] Note note)
        {
            try
            {
                if (index < 0 || index >= StaticDb.Notes.Count)
                {
                    return NotFound($"Note with index {index} not found.");
                }
                if (note == null)
                {
                    return BadRequest("Note cannot be null.");
                }
                if (string.IsNullOrEmpty(note.Text))
                {
                    return BadRequest("Text is a required field.");
                }
                if (note.Tags == null || note.Tags.Count == 0)
                {
                    return BadRequest("At least one tag is required.");
                }
                StaticDb.Notes[index] = note;
                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"an error occured! Contact the admin!");
            }
        }
    }
}
