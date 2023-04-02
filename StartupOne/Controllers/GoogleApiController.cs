using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;
using Microsoft.AspNetCore.Mvc;
using StartupOne.Service;
using Google.Apis.Calendar.v3;

namespace StartupOne.Controllers
{
    [Route("api/[controller]")]
    public class GoogleApiController : Controller
    {
        private GoogleApiService _googleApiService = new GoogleApiService();

        [HttpGet("events/{user}")]
        public IActionResult GetEvents([FromRoute] string user)
        {
            try
            {
                var events = _googleApiService.GetEvents(user);
                return Ok(events.Result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while getting events: {ex.Message}");
            }
        }

        [HttpPost("auth/{user}")]
        public IActionResult Authentic([FromRoute] string user)
        {
            try
            {
                var credential = _googleApiService.Auth(user);
                return Ok(credential.Result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while getting events: {ex.Message}");
            }
        }
    }
}