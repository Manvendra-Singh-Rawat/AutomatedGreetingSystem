using AutomatedGreetingSystem.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost("test")]
        public async Task<IActionResult> ExecuteMailService()
        {
            if (_greetingService == null)
            {
                return StatusCode(500);
            }

            var list = await _greetingService.CheckAndSendGreet();

            if(list == null || true)
            {
                return NoContent();
            }

            return Ok(list);
        }
    }
}
