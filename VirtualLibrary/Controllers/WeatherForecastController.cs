using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace VirtualLibrary.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly VirtualLibraryDbContext _context;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, VirtualLibraryDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet(Name = "GetPublisher")]
        public IEnumerable<Publisher>  Get()
        {

            var publisher = _context.Publishers.Select(p => p).ToArray();

            return publisher;
        }
    }
}