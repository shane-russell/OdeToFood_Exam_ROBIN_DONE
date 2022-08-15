using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OdeToFood.Api.Controllers.Api;
using OdeToFood.Api.Services;
using OdeToFood.Data.DomainClasses;
using OdeToFood.Data.ViewModels;

namespace OdeToFood.Tests.Controllers.Api
{
    public class HomeControllerTests
    {
        private Mock<IRestaurantRepository> _restaurantRepositoryMock;
        private Mock<IReviewRepository> _reviewRepositoryMock;
        private Mock<IReviewFactory> _reviewFactoryMock;
        private Restaurant _newRestaurant;
        private List<Restaurant> _restaurants;
        private HomeController _controller;
        private Review _newReview;
        private List<Review> _reviews;
        private ReviewModel _reviewModel;

        [SetUp]
        public void SetUp()
        {
            _restaurantRepositoryMock = new Mock<IRestaurantRepository>();
            _reviewRepositoryMock = new Mock<IReviewRepository>();
            _reviewFactoryMock = new Mock<IReviewFactory>();

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

            _newReview = new Review
            {
                Body = "Test",
                Id = 1,
                Rating = 8,
                Restaurant = _newRestaurant,
                RestaurantId = 1,
                ReviewerName = "Anonymous"
            };

            _reviews = new List<Review>()
            {
                _newReview
            };

            _reviewModel = new ReviewModel
            {
                Body = "Lekker gegeten",
                Rating = 8,
                ReviewerName = "Anonymous"
            };

            _controller = new HomeController(_restaurantRepositoryMock.Object, _reviewRepositoryMock.Object, _reviewFactoryMock.Object);
        }
        [Test]
        public void Index_ShouldReturnAListOfAllRestaurants()
        {
            //ARRANGE
            _restaurantRepositoryMock.Setup(repository => repository.GetAll()).Returns(_restaurants);

            //ACT
            var result = _controller.Index() as ViewResult;

            //ASSERT
            _restaurantRepositoryMock.Verify(repository => repository.GetAll(), Times.Once);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<ViewResult>());
            Assert.That(result.Model, Is.SameAs(_restaurants));
        }

        [Test]
        public void Index_ShouldReturnNotFoundIfListOfAllRestaurantsDoesNotExist()
        {
            //ACT
            var result = _controller.Index() as NotFoundResult;

            //ASSERT
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public void Details_ShouldShowTheDetailsOfARestaurant()
        {
            //ARRANGE
            _restaurantRepositoryMock.Setup(repository => repository.GetById(It.IsAny<int>())).Returns(_newRestaurant);
            _reviewRepositoryMock.Setup(repository => repository.GetAllAsync()).ReturnsAsync(_reviews);

            //ACT
            var result = _controller.Details(It.IsAny<int>()).Result as ViewResult;

            //ASSERT
            _restaurantRepositoryMock.Verify(repository => repository.GetById(It.IsAny<int>()), Times.Once);
            _reviewRepositoryMock.Verify(repository => repository.GetAllAsync(), Times.Once);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<ViewResult>());
            Assert.That(result.Model, Is.InstanceOf<RestaurantDetails>());
        }

        [Test]
        public void Detail_ShouldRedirectToTheIndexViewIfRestaurantDoesNotExist()
        {
            //ARRANGE
            _restaurantRepositoryMock.Setup(repository => repository.GetById(It.IsAny<int>())).Returns((Restaurant)null);

            //ACT
            var result = _controller.Details(It.IsAny<int>()).Result as RedirectToActionResult;

            //ASSERT
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
            Assert.That(result.ActionName, Is.EqualTo(nameof(_controller.Index)));
        }

        [Test]
        public void AddReview_ShouldCreateANewReview()
        {
            //ARRANGE
            _restaurantRepositoryMock.Setup(repository => repository.GetById(It.IsAny<int>())).Returns(_newRestaurant);

            //ACT
            var result = _controller.AddReview(_newRestaurant.Id, _reviewModel) as RedirectToActionResult;

            //ASSERT
            _restaurantRepositoryMock.Verify(repository => repository.GetById(It.IsAny<int>()), Times.Once);
            _reviewRepositoryMock.Verify(repository => repository.AddAsync(It.IsAny<Review>()), Times.Once);
            _reviewFactoryMock.Verify(factory => factory.Create(It.IsAny<Restaurant>(),It.IsAny<ReviewModel>()),Times.Once);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
            Assert.That(result.ActionName, Is.EqualTo(nameof(_controller.Details)));
        }

        [Test]
        public void AddReview_ShouldReturnViewIfReviewIsInvalid()
        {
            //ARRANGE        
            _controller.ModelState.AddModelError("Id", "The Id is Required");

            //ACT
            var result = _controller.AddReview(It.IsAny<int>(), It.IsAny<ReviewModel>());

            //ASSERT
            Assert.That(result, Is.InstanceOf<ViewResult>());
        }
    }
}
