using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        protected List<string> GetPropertyNames<P>(P entity)
        {
            var result = new List<string>();
            var entityType = _context.Model.FindEntityType(typeof(P));
            foreach (var item in entityType.GetProperties().Select(e => e.GetColumnName()).Where(s => !s.Contains("Id")).ToList())
            {
                result.Add(item);
            }

            return result;
        }

        [HttpGet]
        public void Get()
        {
            _context.Items.Remove(_context.Items.Find(10));
            _context.SaveChanges();
        }
    }
}