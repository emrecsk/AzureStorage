using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.SemanticKernel;
using WebApp.Hubs;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationsController(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }
        [HttpGet]
        public IActionResult Get()
        {
            _hubContext.Clients.All.SendAsync("transferchartdata", "Hello");
            return Ok(new { Message = "Request Completed" });
        }
        [HttpPost]
        public IActionResult CompleteWatermarkProcess(string connectionid)
        {
            _hubContext.Clients.Client(connectionid).SendAsync("NotifyCompleteWaterMarkProcess");
            return Ok(new { Message = "Request Completed" });
        }       
    }
}
