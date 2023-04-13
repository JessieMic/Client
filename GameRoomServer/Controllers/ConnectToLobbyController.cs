using Microsoft.AspNetCore.Mvc;

namespace GameRoomServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConnectToLobbyController: ControllerBase
    {
        [HttpGet(Name = "connectToLobby")]
        public IActionResult Get()
        {
            string ip = HttpContext.Connection.RemoteIpAddress.ToString();
            if (HttpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                ip = HttpContext.Request.Headers["X-Forwarded-For"];
            }
            return Ok(ip);
        }
    }
}
