using Microsoft.AspNetCore.Mvc;
using VirtualLibrary.Utilites.Implementations.Filters.ModelFields;
using VirtualLibrary.Utilites.Interfaces;

namespace VirtualLibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MagazineController : ControllerBase
    {
        private readonly ILogger<MagazineController> _logger;
        private readonly IDataStore<Magazine, MagazineDTO> _dataStore;

        public MagazineController(ILogger<MagazineController> logger, IDataStore<Magazine, MagazineDTO> dataStore)
        {
            _logger = logger;
            _dataStore = dataStore;
        }

        [HttpGet("ordered")]
        public async Task<IActionResult> GetMagazines([FromHeader] string field)
        {
            var IsParsed = FieldParser.TryParseField(field, out var parsedField);

            if (!IsParsed)
            {
                return BadRequest($"{parsedField} field!");
            }
            var result = await _dataStore.GetSortedDataAsync(parsedField);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result.ActionResult);
        }

        [HttpGet]
        public async Task<IActionResult> GetMagazines()
        {
            var result = await _dataStore.GetDataAsync();

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result.ActionResult);
        }

        /*[HttpGet("id")]
        public async Task<IActionResult> GetBookByID([FromQuery] string id)
        {
            var isInteger = int.TryParse(id, out int parsedId);

            if (string.IsNullOrWhiteSpace(id) && !isInteger)
            {
                return BadRequest("Incorect Id");
            }

            var result = await _repository.GetByIdAsync(parsedId);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            var succseedResult = (ActionManagerResponse<Book>)result;
            return Ok(succseedResult.ActionResult);
        }*/

        [HttpPost]
        public async Task<IActionResult> PostMagazine([FromBody] MagazineDTO magazineDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _dataStore.AddDataAsync(magazineDto);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result.ActionResult);
        }

        /*[HttpPut("id")]
        public async Task<ActionResult> PutBook([FromQuery] string id, [FromBody] BookDTO bookDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var isInteger = int.TryParse(id, out int parsedId);

            if (string.IsNullOrWhiteSpace(id) && !isInteger)
            {
                return BadRequest("Incorect Id");
            }

            var result = await _repository.UpdateAsync(parsedId, bookDto);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            var succseedResult = (ActionManagerResponse<Book>)result;
            return Ok(succseedResult.ActionResult);
        }*/

        [HttpDelete("id")]
        public async Task<IActionResult> DeleteMagazine([FromQuery] string id)
        {
            var isInteger = int.TryParse(id, out int parsedId);

            if (string.IsNullOrWhiteSpace(id) && !isInteger)
            {
                return BadRequest("Incorect Id");
            }

            var result = await _dataStore.DeleteDataAsync(parsedId);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result.ActionResult);
        }
    }
}
