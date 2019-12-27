using Microsoft.AspNetCore.Mvc;

namespace DistributedExceptionHandler.Controllers
{
    [Route("exceptionhandler/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        public IActionResult Welcome() 
            => Content($"Welcome to Distributed Exception Handler. This endpoint could be used for future HTTP health checks.");
    }
}