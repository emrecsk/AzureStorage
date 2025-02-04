using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.AI.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.AI.OpenAI;
using Microsoft.SemanticKernel.Connectors.AI.OpenAI.ChatCompletion;
using Microsoft.SemanticKernel.Planners;
using NuGet.Protocol;
using System.Diagnostics;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IKernel _kernel;
        public const string SessionKeyName = "_AISession";        

        public HomeController(ILogger<HomeController> logger, IKernel kernel)
        {
            _logger = logger;
            _kernel = kernel;            
        }

        public IActionResult Index()
        {            
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpPost]
        public async Task<JsonResult> Chat(string message)
        {
            string response = string.Empty;            
            var chatCompletionService2 = _kernel.GetService<IChatCompletion>();                 
            var chat = (OpenAIChatHistory)chatCompletionService2.CreateNewChat(); 
            chat.AddUserMessage(message);            
            response = await chatCompletionService2.GenerateMessageAsync(chat, new OpenAIRequestSettings());
            return Json(response);
        }
    }
}