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
        public List<string> Get()
        {
            var book = _context.BookCopies
                .Include(p => p.Book)
                .Include(p => p.Item)
                .ThenInclude(p => p.Publisher)
                .FirstOrDefault();
            var listOfNames = new List<string>();

            listOfNames.AddRange(GetPropertyNames(book));
            listOfNames.AddRange(GetPropertyNames(book.Book));
            listOfNames.AddRange(GetPropertyNames(book.Item));
            listOfNames.AddRange(GetPropertyNames(book.Item.Publisher));

            return listOfNames;
        }
    }
}