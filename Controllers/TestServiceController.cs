using AutomatedGreetingSystem.Application.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
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

        //[HttpPost]
        //public IActionResult ExecuteMailService()
        //{
        //    if(_greetingService == null)
        //    {
        //        return InternalServcerErr
        //    }
        //    //return View();
        //}
    }
}
