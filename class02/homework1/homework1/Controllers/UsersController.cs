using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace homework1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpGet] 
        public ActionResult<List<string>> Get()
        {
            //return BadRequest(StaticDb.UserNames);
            return Ok(StaticDb.UserNames); // Changed from BadRequest to Ok
        }
        [HttpGet("{index}")] 
        public ActionResult<string> GetByIndex(int index)
        {
            try
            {
                if (index < 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest,
                        "The index has negative value");
                }

                if (index >= StaticDb.UserNames.Count)
                {
                    return StatusCode(StatusCodes.Status404NotFound,
                        $"There is no resourse on index {index}");
                }

                return StatusCode(StatusCodes.Status200OK, StaticDb.UserNames[index]);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occurred. Contact The admin");
            }
        }
    }
}
