using Microsoft.AspNetCore.Mvc;

namespace ChatBackend.Controllers
{
    [ApiController]
    [Route("ping")]
    public class PingController
    {
        [HttpGet]
        public string GetPing()
        {
            return "pong";
        }
    }
}
