using Microsoft.AspNetCore.Mvc;
using VirtualLibrary.Utilites.Implementations;
using VirtualLibrary.Utilites.Interfaces;

namespace VirtualLibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublisherController : ControllerBase
    {
        private readonly ILogger<PublisherController> _logger;
        private readonly IRepository<Publisher, PublisherDTO> _repository;

        public PublisherController(ILogger<PublisherController> logger, RepositoryFactory factory)
        {
            _logger = logger;
            _repository = factory.GetRepository<Publisher, PublisherDTO>();
        }

        [HttpGet]
        public async Task<IActionResult> GetPublisher()
        {
            var result = await _repository.GetAllAsync();

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result.ActionResult);
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetPublisherById([FromQuery] string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("Incorect Id");
            }

            var result = await _repository.GetByIdAsync(int.Parse(id));

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result.ActionResult);
        }

        [HttpPost]
        public async Task<IActionResult> PostPublisher([FromBody]PublisherDTO publisherDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _repository.CreateAsync(publisherDto);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut("id")]
        public async Task<ActionResult> PutPublisher([FromQuery] string id, [FromBody] PublisherDTO publisherDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("Incorect Id");
            }

            var result = await _repository.UpdateAsync(int.Parse(id), publisherDto);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpDelete("id")]
        public async Task<IActionResult> DeletePublisher([FromQuery] string id)
        {
            if(string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("Incorect Id");
            }

            var result = await _repository.DeleteAsync(int.Parse(id));

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
