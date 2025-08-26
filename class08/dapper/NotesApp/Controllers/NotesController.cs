using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotesApp.Dtos.NoteDtos;
using NotesApp.Services.Interfaces;
using NotesApp.Shared.CustomExceptions;

namespace NotesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INoteService _noteService;
        public NotesController(INoteService noteService)
        {
            _noteService = noteService;
        }
        [HttpGet]
        public ActionResult<List<NoteDto>> GetAllNotes()
        {
            try
            {
                var notes = _noteService.GetAllNotes();
                return Ok(notes);
            }
            catch (NoteDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{id}")]
        public ActionResult<NoteDto> GetById(int id)
        {
            try
            {
                var noteDto = _noteService.GetById(id);
                return Ok(noteDto); //status code = 200
            }
            catch (NoteNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }
        [HttpPost("addNote")]
        public IActionResult AddNote([FromBody] AddNoteDto addNoteDto)
        {
            try
            {
                _noteService.AddNote(addNoteDto);
                //return CreatedAtAction(nameof(GetById), addNoteDto);
                return StatusCode(StatusCodes.Status201Created, "New note added");
            }
            catch (NoteDataException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }
        [HttpPut("{id}")]
        public IActionResult UpdateNote(int id, [FromBody] UpdateNoteDto updateNoteDto)
        {
            try
            {
                updateNoteDto.Id = id;
                _noteService.UpdateNote(updateNoteDto);
                return NoContent(); //status code = 204
            }
            catch (NoteNotFoundException ex)
            {
                return NotFound(ex.Message); //status code = 404
            }
            catch (NoteDataException ex)
            {
                return BadRequest(ex.Message); //status code = 400
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }
        [HttpDelete("{id}")] //api/notes/1 - send Id
        public IActionResult Delete(int id)
        {
            try
            {
                _noteService.DeleteNote(id);
                //return NoContent();
                return Ok($"Note with Id {id} deleted successfully");
            }
            catch (NoteNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }
    }
}
