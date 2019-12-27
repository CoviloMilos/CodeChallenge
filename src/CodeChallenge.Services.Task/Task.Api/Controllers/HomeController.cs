using CodeChallenge.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Task.Api.Controllers
{
    [Route("task/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        public IActionResult Welcome() => Content($"Welcome to Task.API");

        [HttpGet("throwEx")]
        public IActionResult ThrowEx()
        {
            throw new CustomException("throwEx_endpoint", "Manual testing publish msg to RabbitMq");
        }
    }
}