using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualLibrary.Controllers;
using VirtualLibrary.Logic.Interface;
using VirtualLibrary.Models;

namespace VirtualLibrary.Tests.Controllers
{
    public class PublisherControllerTests
    {
        private Mock<ILogger<PublisherController>> _logger;
        private Mock<IModelLogic<Publisher, PublisherDTO>> _modelLogic;
        private PublisherController _controller;
        public PublisherControllerTests()
        {
            _logger = new Mock<ILogger<PublisherController>>();
            _modelLogic = new Mock<IModelLogic<Publisher, PublisherDTO>>();

            _controller = new PublisherController(_logger.Object, _modelLogic.Object);
        }

        [Fact]
        public async void GetPublisher_GetResponseSuccessTrue_ReturnOk()
        {
            //Arrange
            var response = new Response<IEnumerable<Publisher>> { Success = true };

            _modelLogic.Setup(x => x.GetDataAsync()).ReturnsAsync(response);

            //Act

            var result = await _controller.GetPublisher();

            //Assert

            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void GetPublisher_GetResponseSuccessFalse_ReturnBadRequest()
        {
            //Arrange
            var response = new Response<IEnumerable<Publisher>> { Success = false };

            _modelLogic.Setup(x => x.GetDataAsync()).ReturnsAsync(response);

            //Act

            var result = await _controller.GetPublisher();

            //Assert

            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async void GetPublisherOrdered_GetResponseSuccessTrue_ReturnOk()
        {
            //Arrange
            var response = new Response<IEnumerable<Publisher>> { Success = true };
            var field = "field";

            _modelLogic.Setup(x => x.GetSortedDataAsync(field)).ReturnsAsync(response);

            //Act

            var result = await _controller.GetPublisher(field);

            //Assert

            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void GetPublisherOrdered_GetResponseSuccessFalse_ReturnBadRequest()
        {
            //Arrange
            var response = new Response<IEnumerable<Publisher>> { Success = false };
            var field = "field";

            _modelLogic.Setup(x => x.GetSortedDataAsync(field)).ReturnsAsync(response);

            //Act

            var result = await _controller.GetPublisher(field);

            //Assert

            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async void GetPublisherById_GetResponseSuccessTrue_ReturnOk()
        {
            //Arrange
            var response = new Response<IEnumerable<Publisher>> { Success = true };
            var id = "0";

            _modelLogic.Setup(x => x.GetDatabyId(int.Parse(id))).ReturnsAsync(response);

            //Act

            var result = await _controller.GetPublisherById(id);

            //Assert

            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void GetPublisherById_GetResponseSuccessFalse_ReturnNotFound()
        {
            //Arrange
            var response = new Response<IEnumerable<Publisher>> { Success = false };
            var id = "0";

            _modelLogic.Setup(x => x.GetDatabyId(int.Parse(id))).ReturnsAsync(response);

            //Act

            var result = await _controller.GetPublisherById(id);

            //Assert

            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async void PostPublisher_GetResponseSuccessTrue_ReturnOk()
        {
            //Arrange
            var response = new Response<IEnumerable<Publisher>> { Success = true };
            var publisher = new PublisherDTO { Name = "name" };

            _modelLogic.Setup(x => x.AddDataAsync(publisher)).ReturnsAsync(response);

            //Act

            var result = await _controller.PostPublisher(publisher);

            //Assert

            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void PostPublisher_GetResponseSuccessFalse_ReturnBadRequest()
        {
            //Arrange
            var response = new Response<IEnumerable<Publisher>> { Success = false };
            var publisher = new PublisherDTO { Name = "name" };

            _modelLogic.Setup(x => x.AddDataAsync(publisher)).ReturnsAsync(response);

            //Act

            var result = await _controller.PostPublisher(publisher);

            //Assert

            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async void PostPublisher_InvalidModel_ReturnBadRequest()
        {
            //Arrange
            var response = new Response<IEnumerable<Publisher>> { Success = false };
            var publisher = new PublisherDTO { Name = null };

            _controller.ModelState.AddModelError("", "");
            //Act

            var result = await _controller.PostPublisher(publisher);

            //Assert

            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async void PutPublisher_GetResponseSuccessTrue_ReturnOk()
        {
            //Arrange
            var response = new Response<IEnumerable<Publisher>> { Success = true };
            var id = "0";
            var publisher = new PublisherDTO { Name = "name" };

            _modelLogic.Setup(x => x.UpdateDataAsync(int.Parse(id) ,publisher)).ReturnsAsync(response);

            //Act

            var result = await _controller.PutPublisher(id, publisher);

            //Assert

            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void PutPublisher_GetResponseSuccessFalse_ReturnNotFound()
        {
            //Arrange
            var response = new Response<IEnumerable<Publisher>> { Success = false };
            var id = "0";
            var publisher = new PublisherDTO { Name = "name" };

            _modelLogic.Setup(x => x.UpdateDataAsync(int.Parse(id), publisher)).ReturnsAsync(response);

            //Act

            var result = await _controller.PutPublisher(id, publisher);

            //Assert

            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async void PutPublisher_InvalidModel_ReturnBadRequest()
        {
            //Arrange
            var response = new Response<IEnumerable<Publisher>> { Success = false };
            var id = "0";
            var publisher = new PublisherDTO { Name = null };

            _controller.ModelState.AddModelError("", "");
            //Act

            var result = await _controller.PutPublisher(id, publisher);

            //Assert

            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async void DeletePublisher_GetResponseSuccessTrue_ReturnOk()
        {
            //Arrange
            var response = new Response<IEnumerable<Publisher>> { Success = true };
            var id = "0";

            _modelLogic.Setup(x => x.DeleteDataAsync(int.Parse(id))).ReturnsAsync(response);

            //Act

            var result = await _controller.DeletePublisher(id);

            //Assert

            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void DeletePublisher_GetResponseSuccessFalse_ReturnNotFound()
        {
            //Arrange
            var response = new Response<IEnumerable<Publisher>> { Success = false };
            var id = "0";

            _modelLogic.Setup(x => x.DeleteDataAsync(int.Parse(id))).ReturnsAsync(response);

            //Act

            var result = await _controller.DeletePublisher(id);

            //Assert

            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}

