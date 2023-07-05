using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmailController : ControllerBase
    {

        private static Dictionary<string, DateTime> _Requests = new();
        private static readonly object _Lock = new();
        private const int TimeLimit = 3;


        [HttpPost]
        public IActionResult Post([FromBody] EmailRequest request)
        {
            //locks the lock to prevent another thread accessing it
            lock (_Lock)
            {
                // check to see if that email has made a request in the last three seconds
                if (_Requests.ContainsKey(request.Email) && (DateTime.Now - _Requests[request.Email]).TotalSeconds < TimeLimit)
                {
                    return StatusCode(429, new ErrorResponse
                    {
                        Message = "Too many requests. Try again later.",
                        LastValidRequestTime = _Requests[request.Email]
                    });
                }

                _Requests[request.Email] = DateTime.Now;
            }

            var response = new EmailResponse
            {
                Email = request.Email,
                ReceivedTime = DateTime.Now
            };

            return Ok(response);
        }
    }
}