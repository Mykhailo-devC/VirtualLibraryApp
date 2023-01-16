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
        private readonly RepositoryFactory _factory;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, VirtualLibraryDbContext context, RepositoryFactory factory)
        {
            _logger = logger;
            _context = context;
            _factory = factory;
        }

        [HttpGet("GetPublisher")]
        public async void Get()
        {
            var repo = _factory.GetRepository<Article, ArticleDTO>();

            var result = await repo.CreateAsync(new ArticleDTO { Author = "123", ItemId = 1, MagazineId = 1, Version = 1});
            Console.WriteLine(result);

        }
    }
}