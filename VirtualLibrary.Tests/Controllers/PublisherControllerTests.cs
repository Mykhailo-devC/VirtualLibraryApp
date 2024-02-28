using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using VirtualLibrary.Controllers;
using VirtualLibrary.Models;
using VirtualLibrary.Tests.Fakes;

namespace VirtualLibrary.Tests.Controllers
{
    public class PublisherControllerTests
    {
        private Mock<ILogger<PublisherController>> _logger;

        private PublisherController _controllerWithData;
        private PublisherController _controllerEmptyData;
        private PublisherController _controllerWithError;

        private IEnumerable<Publisher> _initData;
        public PublisherControllerTests()
        {
            _logger = new Mock<ILogger<PublisherController>>();

            _initData = new List<Publisher>
                {
                    new Publisher {Id = 1, Name = "John"},
                    new Publisher {Id = 2, Name = "Mike"},
                    new Publisher {Id = 3, Name = "Chris"}
                };

            _controllerWithData = new PublisherController(_logger.Object,
                new ModelLogicFake<Publisher, PublisherDTO>(_initData));

            _controllerEmptyData = new PublisherController(_logger.Object,
                new ModelLogicFake<Publisher, PublisherDTO>(new List<Publisher>()));

            _controllerWithError = new PublisherController(_logger.Object,
                new ModelLogicFake<Publisher, PublisherDTO>(null));
        }

        [Fact]
        public async void GetPublisher_InitDataNotEmpty_ReturnOkWithPublisherList()
        {
            var result = await _controllerWithData.GetPublisher() as OkObjectResult;
            var resultData = (result?.Value as Response<IEnumerable<Publisher>>)?.Data ?? Enumerable.Empty<Publisher>();

            Assert.NotNull(result);
            Assert.NotEmpty(resultData);
        }

        [Fact]
        public async void GetPublisher_InitDataEmpty_ReturnOkWithEmpryList()
        {
            var result = await _controllerEmptyData.GetPublisher() as OkObjectResult;
            var resultData = (result?.Value as Response<IEnumerable<Publisher>>)?.Data;

            Assert.NotNull(result);
            Assert.Empty(resultData);
        }

        [Fact]
        public async void GetPublisher_FailGetData_ReturnBadRequest()
        {
            var result = await _controllerWithError.GetPublisher() as BadRequestObjectResult;
            var resultData = (result?.Value as Response<IEnumerable<Publisher>>)?.Data;

            Assert.NotNull(result);
            Assert.Null(resultData);
        }

        [Theory]
        [InlineData("Id")]
        [InlineData("Name")]
        public async void GetPublisherOrdered_InitDataNotEmpty_ValidProperty_ReturnOkWithPublisherList(string property)
        {
            var result = await _controllerWithData.GetPublisher(property) as OkObjectResult;
            var resultData = (result?.Value as Response<IEnumerable<Publisher>>)?.Data ?? Enumerable.Empty<Publisher>();

            Assert.NotNull(result);
            Assert.NotEmpty(resultData);
        }

        [Theory]
        [InlineData("1")]
        [InlineData(null)]
        public async void GetPublisherOrdered_InitDataNotEmpty_InvalidProperty_ReturnBadRequest(string property)
        {
            var result = await _controllerWithData.GetPublisher(property) as BadRequestObjectResult;
            var resultData = (result?.Value as Response<IEnumerable<Publisher>>)?.Data;

            Assert.NotNull(result);
            Assert.Null(resultData);
        }

        [Fact]
        public async void GetPublisherOrdered_InitDataEmpty_ValidProperty_ReturnOkWithEmptyList()
        {
            var result = await _controllerEmptyData.GetPublisher("Name") as OkObjectResult;
            var resultData = (result?.Value as Response<IEnumerable<Publisher>>)?.Data;

            Assert.NotNull(result);
            Assert.Empty(resultData);
        }

        [Fact]
        public async void GetPublisherOrdered_FailGetData_ValidProperty_ReturnBadRequest()
        {
            var result = await _controllerWithError.GetPublisher("Name") as BadRequestObjectResult;
            var resultData = (result?.Value as Response<IEnumerable<Publisher>>)?.Data;

            Assert.NotNull(result);
            Assert.Null(resultData);
        }

        [Fact]
        public async void GetPublisherOrdered_FailGetData_ReturnBadRequest()
        {
            var result = await _controllerWithError.GetPublisher("Name") as BadRequestObjectResult;
            var resultData = (result?.Value as Response<IEnumerable<Publisher>>)?.Data;

            Assert.NotNull(result);
            Assert.Null(resultData);
        }

        [Theory]
        [InlineData("1")]
        [InlineData("2")]
        public async void GetPublisherById_InitDataNotEmpty_ValidId_ReturnOkWithPublisher(string id)
        {
            var result = await _controllerWithData.GetPublisherById(id) as OkObjectResult;
            var resultData = (result?.Value as Response<Publisher>)?.Data;

            Assert.NotNull(result);
            Assert.NotNull(resultData);
            Assert.Equal(resultData.Id.ToString(), id);
        }


        [Theory]
        [InlineData("0")]
        [InlineData(null)]
        public async void GetPublisherById_InitDataNotEmpty_InvalidId_ReturnNotFound(string id)
        {
            var result = await _controllerWithData.GetPublisherById(id) as NotFoundObjectResult;
            var resultData = (result?.Value as Response<Publisher>)?.Data;

            Assert.NotNull(result);
            Assert.Null(resultData);
        }

        [Fact]
        public async void GetPublisherById_InitDataEmpty_ValidId_ReturnNotFound()
        {
            var result = await _controllerEmptyData.GetPublisherById("1") as NotFoundObjectResult;
            var resultData = (result?.Value as Response<Publisher>)?.Data;

            Assert.NotNull(result);
            Assert.Null(resultData);
        }

        [Fact]
        public async void GetPublisherById_FailGetData_ReturnNotFound()
        {
            var result = await _controllerWithError.GetPublisherById("1") as NotFoundObjectResult;
            var resultData = (result?.Value as Response<IEnumerable<Publisher>>)?.Data;

            Assert.NotNull(result);
            Assert.Null(resultData);
        }

        [Theory]
        [InlineData("Tom")]
        [InlineData("333")]
        public async void PostPublisher_InitDataNotEmpty_ValidDTO_ReturnOkWithNewPublisher(string name)
        {
            PublisherDTO publisher = new PublisherDTO { Name = name };

            var result = await _controllerWithData.PostPublisher(publisher) as OkObjectResult;
            var resultData = (result?.Value as Response<Publisher>)?.Data;

            Assert.NotNull(result);
            Assert.NotNull(resultData);
            Assert.Contains(resultData, _initData);
        }

        [Fact]
        public async void PostPublisher_InitDataNotEmpty_InvalidDTO_ReturnBadRequest()
        {
            PublisherDTO publisher = null;
            _controllerWithData.ModelState.AddModelError("", "");

            var result = await _controllerWithData.PostPublisher(publisher) as BadRequestObjectResult;
            var resultData = (result?.Value as Response<Publisher>)?.Data;

            Assert.NotNull(result);
            Assert.Null(resultData);
        }

        [Fact]
        public async void PostPublisher_FailPostData_ReturnBadRequest()
        {
            PublisherDTO publisher = null;

            var result = await _controllerWithError.PostPublisher(publisher) as BadRequestObjectResult;
            var resultData = (result?.Value as Response<IEnumerable<Publisher>>)?.Data;

            Assert.NotNull(result);
            Assert.Null(resultData);
        }

        [Theory]
        [InlineData("1","Tom")]
        [InlineData("2","333")]
        public async void PutPublisher_InitDataNotEmpty_ValidDTO_ValidId_ReturnOkWithUpdatedPublisher(string id, string name)
        {
            PublisherDTO publisher = new PublisherDTO { Name = name };

            var result = await _controllerWithData.PutPublisher(id, publisher) as OkObjectResult;
            var resultData = (result?.Value as Response<Publisher>)?.Data;

            Assert.NotNull(result);
            Assert.NotNull(resultData);
            Assert.Contains(_initData, x => x.Name == name && x.Id.ToString() == id);
        }

        [Theory]
        [InlineData(null, "Tom")]
        [InlineData("0", "333")]
        public async void PutPublisher_InitDataNotEmpty_ValidDTO_InvalidId_ReturnNotFound(string id, string name)
        {
            PublisherDTO publisher = new PublisherDTO { Name = name };

            var result = await _controllerWithData.PutPublisher(id, publisher) as NotFoundObjectResult;
            var resultData = (result?.Value as Response<Publisher>)?.Data;

            Assert.NotNull(result);
            Assert.Null(resultData);
        }

        [Fact]
        public async void PutPublisher_InitDataNotEmpty_InvalidDTO_ValidId_ReturnBadRequest()
        {
            PublisherDTO publisher = null;
            _controllerWithData.ModelState.AddModelError("", "");

            var result = await _controllerWithData.PutPublisher("1", publisher) as BadRequestObjectResult;
            var resultData = (result?.Value as Response<Publisher>)?.Data;

            Assert.NotNull(result);
            Assert.Null(resultData);
        }

        [Fact]
        public async void PutPublisher_InitDataEmpty_ValidDTO_ValidId_ReturnNotFound()
        {
            PublisherDTO publisher = new PublisherDTO { Name = "Name" };

            var result = await _controllerEmptyData.PutPublisher("1", publisher) as NotFoundObjectResult;
            var resultData = (result?.Value as Response<Publisher>)?.Data;

            Assert.NotNull(result);
            Assert.Null(resultData);
        }

        [Fact]
        public async void PutPublisher_FailPutData_ReturnNotFound()
        {
            PublisherDTO publisher = null;

            var result = await _controllerWithError.PutPublisher("1", publisher) as NotFoundObjectResult;
            var resultData = (result?.Value as Response<IEnumerable<Publisher>>)?.Data;

            Assert.NotNull(result);
            Assert.Null(resultData);
        }

        [Theory]
        [InlineData("1")]
        [InlineData("2")]
        public async void DeletePublisher_InitDataNotEmpty_ValidId_ReturnOkWithDeletedPublisher(string id)
        {
            var result = await _controllerWithData.DeletePublisher(id) as OkObjectResult;
            var resultData = (result?.Value as Response<Publisher>)?.Data;

            Assert.NotNull(result);
            Assert.NotNull(resultData);
            Assert.DoesNotContain(_initData, x => x.Id.ToString() == id);
        }

        [Theory]
        [InlineData("0")]
        [InlineData(null)]
        public async void DeletePublisher_InitDataNotEmpty_InvalidId_ReturnNotFound(string id)
        {
            var result = await _controllerWithData.DeletePublisher(id) as NotFoundObjectResult;
            var resultData = (result?.Value as Response<Publisher>)?.Data;

            Assert.NotNull(result);
            Assert.Null(resultData);
        }

        [Fact]
        public async void DeletePublisher_InitDataEmpty_ValidId_ReturnNotFound()
        {
            var result = await _controllerEmptyData.DeletePublisher("1") as NotFoundObjectResult;
            var resultData = (result?.Value as Response<Publisher>)?.Data;

            Assert.NotNull(result);
            Assert.Null(resultData);
        }

        [Fact]
        public async void DeletePublisher_FailDeleteData_ReturnNotFound()
        {
            var result = await _controllerWithError.DeletePublisher("1") as NotFoundObjectResult;
            var resultData = (result?.Value as Response<Publisher>)?.Data;

            Assert.NotNull(result);
            Assert.Null(resultData);
        }
    }
}

