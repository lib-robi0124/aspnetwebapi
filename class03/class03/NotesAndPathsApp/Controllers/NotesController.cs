using Microsoft.AspNetCore.Mvc;
using NotesAndTagssApp.Models;

namespace NotesAndTagssApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<Note>> Get()
        {
            try
            {
                return Ok(StaticDb.Notes);
            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, $"an error occured");
            }
        }
        [HttpGet("{index}")]
        public ActionResult<Note> GetNoteByIndex(int index)
        {
            try
            {
                if (index < 0)
                {
                    return BadRequest("the index can not be negative");

                }
                if (index >= StaticDb.Notes.Count) {
                    return NotFound($"index not found");
                }
            catch (Exception)
            {

                throw;
            }
            return Ok(StaticDb.Notes[index]);
        }
        [HttpGet("queryString")]
        public ActionResult<Note> GetByQueryString(int? index)
        {
            try
            {
                if (index == null)
                {
                    return BadRequest("index need parameter");
                }
                if (index < 0)
                {
                    return BadRequest($"the index can not be negative {index}");

                }
                if (index >= StaticDb.Notes.Count)
                {
                    return NotFound();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
