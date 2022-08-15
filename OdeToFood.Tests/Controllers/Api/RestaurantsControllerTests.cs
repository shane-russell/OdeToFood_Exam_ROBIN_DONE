using NUnit.Framework;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OdeToFood.Api.Controllers.Api;
using OdeToFood.Api.Services;
using OdeToFood.Data.DomainClasses;

namespace OdeToFood.Tests.Controllers.Api
{
    public class RestaurantsControllerTests
    {
        private Mock<IRestaurantRepository> _restaurantRepositoryMock;
        private Restaurant _newRestaurant;
        private List<Restaurant> _restaurants;
        private RestaurantModel _restaurantModel;
        private RestaurantsController _controller;
        private RestaurantModel _updateRestaurantModel;

        [SetUp]
        public void SetUp()
        {
            _restaurantRepositoryMock = new Mock<IRestaurantRepository>();

            _newRestaurant = new Restaurant
            {
                City = "Hasselt",
                Country = "Belgium",
                Id = 1,
                Name = "GoodFood"
            };

            _restaurants = new List<Restaurant>()
            {
                _newRestaurant
            };

            _restaurantModel = new RestaurantModel
            {
                City = "Sint-Truiden",
                Country = "Belgium",
                Name = "PastaPronto"          
            };

            _updateRestaurantModel = new RestaurantModel
            {
                Name = "Bull's Eye"
            };

            _controller = new RestaurantsController(_restaurantRepositoryMock.Object);
        }

        [Test]
        public void GetAll_ReturnsAllRestaurantsFromRepository()
        {
            //ARRANGE
            _restaurantRepositoryMock.Setup(x => x.GetAll()).Returns(_restaurants);

            //ACT
            var result = _controller.GetAll() as OkObjectResult;

            //ASSERT
            _restaurantRepositoryMock.Verify(x => x.GetAll(), Times.Once);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.SameAs(_restaurants));
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public void GetById_ReturnsRestaurantIfItExists()
        {
            //ARRANGE
            _restaurantRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(_newRestaurant);

            //ACT
            var result = _controller.GetById(_newRestaurant.Id) as OkObjectResult;

            //ASSERT
            _restaurantRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.SameAs(_newRestaurant));
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public void GetById_ReturnsNotFoundIfItDoesNotExists()
        {
            //ACT
            var result = _controller.GetById(It.IsAny<int>());

            //ASSERT
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public void Post_ValidRestaurantIsSavedInRepository()
        {
            //ACT
            var result = _controller.Create(_restaurantModel) as CreatedAtActionResult;

            //ASSERT
            _restaurantRepositoryMock.Verify(x => x.Add(It.IsAny<Restaurant>()), Times.Once);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ActionName, Is.EqualTo(nameof(_controller.GetById)));
            Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());

            var item = result.Value as Restaurant;
            Assert.That(item, Is.Not.Null);
            Assert.That(item, Is.InstanceOf<Restaurant>());
        }

        [Test]
        public void Post_InValidRestaurantCausesBadRequest()
        {
            //ARRANGE        
            _controller.ModelState.AddModelError("Id", "The Id is Required");

            //ACT
            var result = _controller.Create(It.IsAny<RestaurantModel>());

            //ASSERT
            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }

        [Test]
        public void Put_ExistingRestaurantIsSavedInRepository()
        {
            //ARRANGE
            int restaurantId = 1;

            //ACT
            var result = _controller.Update(restaurantId, _updateRestaurantModel) as OkResult;

            //ASSERT
            _restaurantRepositoryMock.Verify(x => x.Update(It.IsAny<Restaurant>()), Times.Once);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<OkResult>());
        }

        [Test]
        public void Put_InValidRestaurantModelStateCausesBadRequest()
        {
            //ARRANGE
            _controller.ModelState.AddModelError("Id", "The Id is Required");

            //ACT
            var result = _controller.Update(It.IsAny<int>(), It.IsAny<RestaurantModel>());

            //ASSERT
            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }

        [Test]
        public void Delete_ExistingRestaurantIsDeletedFromRepository()
        {
            //ARRANGE
            _restaurantRepositoryMock.Setup(x => x.GetById(_newRestaurant.Id)).Returns(_newRestaurant);

            //ACT
            var result = _controller.Delete(_newRestaurant.Id) as OkResult;

            //Assert
            _restaurantRepositoryMock.Verify(repository => repository.Delete(It.IsAny<int>()), Times.Once);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<OkResult>());
        }

        [Test]
        public void Delete_NonExistingRestaurantReturnsNotFound()
        {
            //ACT
            var result = _controller.Delete(It.IsAny<int>()) as NotFoundResult;

            //ASSERT
            _restaurantRepositoryMock.Verify(repository => repository.GetById(It.IsAny<int>()), Times.Once);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }
    }
}
