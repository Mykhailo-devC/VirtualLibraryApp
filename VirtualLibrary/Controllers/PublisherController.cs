using Microsoft.AspNetCore.Mvc;
using VirtualLibrary.Logic.Interface;

namespace VirtualLibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublisherController : ControllerBase
    {
        private readonly ILogger<PublisherController> _logger;
        private readonly IModelLogic<Publisher, PublisherDTO> _dataStore;

        public PublisherController(ILogger<PublisherController> logger, IModelLogic<Publisher, PublisherDTO> dataStore)
        {
            _logger = logger;
            _dataStore =  dataStore;
        }

        [HttpGet("ordered/{field}")]
        public async Task<IActionResult> GetPublisher(string field)
        {
            var result = await _dataStore.GetSortedDataAsync(field);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetPublisher()
        {
            var result = await _dataStore.GetDataAsync();

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetPublisherById(string id)
        {
            var result = await _dataStore.GetDatabyId(int.Parse(id));

            if(!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> PostPublisher([FromBody]PublisherDTO publisherDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _dataStore.AddDataAsync(publisherDto);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut("id")]
        public async Task<ActionResult> PutPublisher(string id, [FromBody] PublisherDTO publisherDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _dataStore.UpdateDataAsync(int.Parse(id), publisherDto);

            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        [HttpDelete("id")]
        public async Task<IActionResult> DeletePublisher(string id)
        {
            var result = await _dataStore.DeleteDataAsync(int.Parse(id));

            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }
    }
}
