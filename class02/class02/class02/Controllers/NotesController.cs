using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace class02.Controllers
{
    [Route("api/[controller]")] //http://localhost:[port]/api/notes 
    [ApiController]
    public class NotesController : ControllerBase
    {
        [HttpGet] //http://localhost:[port]/api/notes 
        public ActionResult<List<string>> Get()
        {
            //return StatusCode(StatusCodes.Status200OK, StaticDb.SimpleNotes);
            return Ok(StaticDb.SimpleNotes);
        }
        [HttpGet("{index}")]
        public ActionResult<string> GetByIndex(int index)
        {
            try
            {
                if (index < 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "The index has negative value");
                }

                if (index <= StaticDb.SimpleNotes.Count)
                {
                    return StatusCode(StatusCodes.Status404NotFound, $"There is no resourse on index {index}");
                }
                return StatusCode(StatusCodes.Status200OK, StaticDb.SimpleNotes[index]);
            }
            catch (Exception e) //for some thing what we can not catch 
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "an error occurred.Contact admin");
            }
        }
        [HttpPost]
        public ActionResult Post([FromBody] string newNote)
        {
            try
            {
                using (StreamReader reader = new StreamReader(Request.Body))
                {
                    //string newNote = reader.ReadToEnd();

                    if (string.IsNullOrEmpty(newNote))
                    {
                        return BadRequest("the body can not be empty!");
                    }
                    StaticDb.SimpleNotes.Add(newNote);
                    return StatusCode(StatusCodes.Status201Created, "the new note added");
                }
            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "an error occurred.Contact admin");
            }
        }
    }
}
