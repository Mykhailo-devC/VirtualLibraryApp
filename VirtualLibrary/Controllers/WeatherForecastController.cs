using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VirtualLibrary.Utilites;

namespace VirtualLibrary.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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

        [HttpPost("GetPublisher")]
        public async void Get(ArticleDTO articleDTO)
        {
            var dto = articleDTO;
            return;
            
        }
    }
}