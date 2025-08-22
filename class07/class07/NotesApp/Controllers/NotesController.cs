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
                var note = _noteService.GetById(id);
                return Ok(note);
            }
            catch (NoteNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpPost]
        public ActionResult AddNote([FromBody] AddNoteDto addNoteDto)
        {
            try
            {
                _noteService.AddNote(addNoteDto);
                return CreatedAtAction(nameof(GetById), addNoteDto);
            }
            catch (NoteDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public ActionResult UpdateNote(int id, [FromBody] UpdateNoteDto updateNoteDto)
        {
            try
            {
                updateNoteDto.Id = id;
                _noteService.UpdateNote(updateNoteDto);
                return NoContent();
            }
            catch (NoteNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                _noteService.DeleteNote(id);
                return NoContent();
            }
            catch (NoteNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
