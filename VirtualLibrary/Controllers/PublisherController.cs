﻿using Microsoft.AspNetCore.Mvc;
using VirtualLibrary.Utilites.Implementations.Filters.ModelFields;
using VirtualLibrary.Utilites.Interfaces;

namespace VirtualLibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublisherController : ControllerBase
    {
        private readonly ILogger<PublisherController> _logger;
        private readonly IDataStore<Publisher, PublisherDTO> _dataStore;

        public PublisherController(ILogger<PublisherController> logger, IDataStore<Publisher, PublisherDTO> dataStore)
        {
            _logger = logger;
            _dataStore =  dataStore;
        }

        [HttpGet("ordered")]
        public async Task<IActionResult> GetPublisher([FromHeader] string field)
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
        public async Task<IActionResult> GetPublisher()
        {
            var result = await _dataStore.GetDataAsync();

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result.ActionResult);
        }

        /*[HttpGet("id")]
        public async Task<IActionResult> GetPublisherById([FromQuery] string id)
        {
            var isInteger = int.TryParse(id, out int parsedId);

            if (string.IsNullOrWhiteSpace(id) && !isInteger)
            {
                return BadRequest("Incorect Id");
            }

            var result = await _dataStore.GetDataAsync();

            if(!result.Success)
            {
                return BadRequest(result);
            }

            var succseedResult = (ActionManagerResponse<Publisher>)result;
            return Ok(succseedResult.ActionResult);
        }*/

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

            return Ok(result.ActionResult);
        }

        [HttpPut("id")]
        public async Task<ActionResult> PutPublisher([FromQuery] string id, [FromBody] PublisherDTO publisherDto)
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

            var result = await _dataStore.UpdateDataAsync(parsedId, publisherDto);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result.ActionResult);
        }

        [HttpDelete("id")]
        public async Task<IActionResult> DeletePublisher([FromQuery] string id)
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
