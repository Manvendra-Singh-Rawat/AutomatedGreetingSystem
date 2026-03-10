using AutomatedGreetingSystem.Application.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AutomatedGreetingSystem.Controllers
{
    [ApiController]
    [Route("test/[controller]")]
    public class TestServiceController : ControllerBase
    {
        private readonly IGreetingService _greetingService;
        public TestServiceController(IGreetingService greetingService)
        {
            _greetingService = greetingService;
        }

        [HttpPost]
        public async Task<IActionResult> ExecuteMailService()
        {
            if (_greetingService == null)
            {
                return StatusCode(500);
            }

            await _greetingService.CheckAndSendGreet();

            return Ok();
        }
    }
}
