using Avenga.NotesApp.Dtos.NoteDtos;
using Avenga.NotesApp.Services.Interfaces;
using Avenga.NotesApp.Shared.CustomExceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Avenga.NotesApp.Controllers
{
    [Authorize]
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
        public ActionResult<List<NoteDto>> GetAll()
        {
            try
            {
                var notes = _noteService.GetAllNotes();
                Log.Information("All notes successfully retrieved.");
                return Ok(notes);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred, contact the Admin!");
            }
        }

        [HttpGet("{id}")]
        public ActionResult<NoteDto> GetById(int id)
        {
            try
            {
                var noteDto = _noteService.GetById(id); //potential NoteNotFounException
                Log.Information($"Note with id {id} successfully retrieved.");
                return Ok(noteDto); // status code => 200
            }
            catch (NoteNotFoundException ex)
            {
                Log.Warning("th requested note was not found");
                return NotFound(ex.Message); // status code => 404
            }
            catch (Exception ex)
            {
                Log.Error($"An error occurred while retrieving the note.{ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred, contact the Admin!");
            }
        }

        [HttpPost("addNote")]
        public IActionResult AddNote([FromBody] AddNoteDto addNoteDto)
        {
            try
            {
                _noteService.AddNote(addNoteDto);
                Log.Information("New Note Added");
                return StatusCode(StatusCodes.Status201Created, "New Note Added");
            }
            catch (NoteDataException ex)
            {
                Log.Warning($"Invalid data provided for adding a note.{ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                Log.Error($"An error occurred while retrieving the note.{ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred, contact the Admin!");
            }
        }

        [HttpPut]
        public IActionResult UpdateNote([FromBody] UpdateNoteDto updateNoteDto)
        {
            try
            {
                _noteService.UpdateNote(updateNoteDto);
                Log.Information($"Note with id {updateNoteDto.Id} successfully updated.");
                return NoContent(); // 204
            }
            catch (NoteNotFoundException ex)
            {
                Log.Warning("The requested note was not found");
                return NotFound(ex.Message); // 404
            }
            catch (NoteDataException ex)
            {
                Log.Warning($"Invalid data provided for updating a note.{ex.Message}");
                return BadRequest(ex.Message); //400
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred, contact the Admin!"); // 500
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteNote(int id)
        {
            try
            {
                _noteService.DeleteNote(id);
                Log.Information($"Note with id {id} successfully deleted.");
                return Ok($"Note with id {id} successfully deleted!");
            }
            catch (NoteNotFoundException e)
            {
                Log.Warning("The requested note was not found");
                return NotFound(e.Message);
            }
            catch (Exception)
            {
                Log.Error($"An error occurred while deleting the note.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred, contact the Admin!"); // 500
            }
        }
    }
}
