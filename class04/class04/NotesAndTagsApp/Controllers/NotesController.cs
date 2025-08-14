using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotesAndTagsApp.DTOs;

namespace NotesAndTagsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<NoteDto>> GetAll()
        {
            try
            {
                var notesDb = StaticDb.Notes;
                var notesDto = notesDb.Select(x => new NoteDto
                { //transfer from to
                    Priority = x.Priority,
                    Text = x.Text,
                    User = $"{x.User.FirstName} {x.User.LastName}",
                    Tags = x.Tags.Select(t => t.Name).ToList()
                }).ToList();
                return Ok(notesDto);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured, contact admin");
            }
        }
        [HttpGet("{id}")]
        public ActionResult<NoteDto> GetById(int id)
        {
            try
            {
                if (id < 0)
                {
                    return BadRequest("the id cannot be negative");
                }
                
                var noteDb = StaticDb.Notes.FirstOrDefault(x => x.Id == id);
                if (noteDb == null)
                {
                    return NotFound($"note with id: {id} does not exist");
                }

                var noteDto = new NoteDto
                { //transfer from to //mapiranje
                    Priority = noteDb.Priority,
                    Text = noteDb.Text,
                    User = $"{noteDb.User.FirstName} {noteDb.User.LastName}",
                    Tags = noteDb.Tags.Select(t => t.Name).ToList()
                };
                return Ok(noteDto);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured, contact admin");
              
            }
        }
        [HttpGet("findbyid")]
        public ActionResult<NoteDto> FindNyId(int id)
        {
            try
            {
                if (id < 0)
                {
                    return BadRequest("the id cannot be negative");
                }

                var noteDb = StaticDb.Notes.FirstOrDefault(x => x.Id == id);
                if (noteDb == null)
                {
                    return NotFound($"note with id: {id} does not exist");
                }

                var noteDto = new NoteDto
                { //transfer from to //mapiranje
                    Priority = noteDb.Priority,
                    Text = noteDb.Text,
                    User = $"{noteDb.User.FirstName} {noteDb.User.LastName}",
                    Tags = noteDb.Tags.Select(t => t.Name).ToList()
                };
                return Ok(noteDto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured, contact admin");

            }
        }
        [HttpPut]
        public IActionResult Put
    }
}
