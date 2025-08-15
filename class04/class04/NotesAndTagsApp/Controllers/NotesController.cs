using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotesAndTagsApp.DTOs;
using NotesAndTagsApp.Models;

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
                //DB
                var notesDb = StaticDb.Notes;
                //mapiranje
                var notesDto = notesDb.Select(x => new NoteDto
                { 
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
                
                var noteDb = StaticDb.Notes.SingleOrDefault(x => x.Id == id);
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
        [HttpGet("findbyid")] //http://localhost:[port]/api/notes/findById?id=1
        public ActionResult<NoteDto> FindNyId(int id)
        {
            try
            {
                if (id < 0)
                {
                    return BadRequest("the id cannot be negative");
                }

                var noteDb = StaticDb.Notes.SingleOrDefault(x => x.Id == id);
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
        public IActionResult Put([FromBody] UpdateNoteDto updateNoteDto)
        {
            try
            {
                var noteDb = StaticDb.Notes.FirstOrDefault(x => x.Id == updateNoteDto.Id);
                if (noteDb == null)
                {
                    return NotFound($"the note with id {updateNoteDto.Id} was not found");
                }
                if (string.IsNullOrEmpty(updateNoteDto.Text))
                {
                    return BadRequest("Text is requered field");
                }

                var userDb = StaticDb.Users.FirstOrDefault(x => x.Id == updateNoteDto.UserId);
                if (userDb == null)
                {
                    return NotFound($"user with id: {updateNoteDto.UserId} was not found");
                }

                var tags = new List<Tag>();
                foreach (int tagId in updateNoteDto.TagIds)
                {
                    var tagDb = StaticDb.Tags.FirstOrDefault(x => x.Id == tagId);

                    if (tagDb == null)
                    {
                        return NotFound($"Tag with id {tagId} was not found!");
                    }
                    tags.Add(tagDb);

                }
                noteDb.Text = updateNoteDto.Text;
                noteDb.Priority = updateNoteDto.Priority;
                noteDb.User = userDb;
                noteDb.UserId = userDb.Id;
                noteDb.Tags = tags;

                return StatusCode(StatusCodes.Status204NoContent, "note update");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured, contact admin");
            }
        }
        [HttpPost("addNote")]
        public IActionResult AddNote([FromBody] AddNoteDto addNoteDto)
        {
            try
            {
                if (string.IsNullOrEmpty(addNoteDto.Text))
                {
                    return BadRequest("Text is requered field");
                }
                var userDb = StaticDb.Users.FirstOrDefault(x => x.Id == addNoteDto.UserId);
                if (userDb == null)
                {
                    return NotFound($"user with name: {addNoteDto.UserId} was not found");
                }
                var tags = new List<Tag>();
                foreach (int tagId in addNoteDto.TagIds)
                {
                    var tagDb = StaticDb.Tags.FirstOrDefault(x => x.Id == tagId);
                    if (tagDb == null)
                    {
                        return NotFound($"Tag with name {tagId} was not found!");
                    }
                    tags.Add(tagDb);
                }
                var newNote = new Note
                {
                    Id = StaticDb.Notes.Count + 1, //simple id generation
                    Text = addNoteDto.Text,
                    Priority = addNoteDto.Priority,
                    User = userDb,
                    UserId = userDb.Id,
                    Tags = tags
                };
                StaticDb.Notes.Add(newNote);
                return StatusCode(StatusCodes.Status201Created, "note created");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured, contact admin");
            }
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteNoteById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Id has invalid value");
                }

                var noteDb = StaticDb.Notes.FirstOrDefault(x => x.Id == id);
                if (noteDb == null)
                {
                    return NotFound($"Note with id: {id} was not found!");
                }

                StaticDb.Notes.Remove(noteDb);
                return Ok("Note was deleted!");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occured, contact admin!");
            }
        }
        [HttpGet("user/{userId}")] // http://localhost:[port]/api/notes/user/1
        public ActionResult<List<NoteDto>> GetNotesByUserId(int userId)
        {
            try
            {
                if (userId <= 0)
                {
                    return BadRequest("User id has invalid value");
                }
                var userDb = StaticDb.Users.FirstOrDefault(x => x.Id == userId);
                if (userDb == null)
                {
                    return NotFound($"User with id: {userId} was not found!");
                }
                var notesDb = StaticDb.Notes.Where(x => x.UserId == userId).ToList();
                if (!notesDb.Any())
                {
                    return NotFound($"No notes found for user with id: {userId}");
                }
                var notesDto = notesDb.Select(x => new NoteDto
                {
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
    }
}
