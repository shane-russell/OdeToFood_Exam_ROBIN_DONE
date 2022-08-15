using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OdeToFood.Api.Controllers.Api;
using OdeToFood.Api.Services;
using OdeToFood.Data.DomainClasses;

namespace OdeToFood.Tests.Controllers.Api
{
    public class ReviewsControllerTests
    {
        private ReviewsController _controller;
        private Mock<IReviewRepository> _reviewRepositoryMock;
        private Review _newReview;
        private List<Review> _reviews;
        private ReviewModel _reviewModel;

        [SetUp]
        public void SetUp()
        {
            _reviewRepositoryMock = new Mock<IReviewRepository>();
            _controller = new ReviewsController(_reviewRepositoryMock.Object);
            _newReview = new Review
            {
                Body = "Test",
                Id = 1,
                Rating = 8,
                Restaurant = new Restaurant { City = "Hasselt", Country = "België", Id = 1, Name = "PizzaPlace" },
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
                Restaurant = new Restaurant { City = "Sint-Truiden", Country = "België", Id = 1, Name = "MeatHouse" },
                RestaurantId = 1,
                ReviewerName = "Anonymous"
            };
        }

        [Test]
        public void GetAll_ReturnsAllReviewsFromRepository()
        {
            //ARRANGE
            _reviewRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(_reviews);

            //ACT
            var result = _controller.GetAll().Result as OkObjectResult;

            //ASSERT
            _reviewRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.SameAs(_reviews));
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public void GetById_ReturnsReviewIfItExists()
        {
            //ARRANGE
            _reviewRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(_newReview);

            //ACT
            var result = _controller.GetById(_newReview.Id).Result as OkObjectResult;

            //ASSERT
            _reviewRepositoryMock.Verify(repository => repository.GetByIdAsync(It.IsAny<int>()), Times.Once);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.SameAs(_newReview));
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public void GetById_ReturnsNotFoundIfItDoesNotExists()
        {
            //ACT
            var result = _controller.GetById(It.IsAny<int>()).Result;

            //ASSERT
            _reviewRepositoryMock.Verify(repository => repository.GetByIdAsync(It.IsAny<int>()), Times.Once);
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public void Post_ValidReviewIsSavedInRepository()
        {
            //ACT
            var result = _controller.Create(_reviewModel).Result as CreatedAtActionResult;

            //ASSERT
            _reviewRepositoryMock.Verify(repository => repository.AddAsync(It.IsAny<Review>()), Times.Once());
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ActionName, Is.EqualTo(nameof(_controller.GetById)));

            //todo how to check on the RouteValues??

            var item = result.Value as Review;
            Assert.That(item, Is.Not.Null);
            Assert.That(item, Is.InstanceOf<Review>());
        }

        [Test]
        public void Post_InValidReviewCausesBadRequest()
        {
            //ARRANGE
            _controller.ModelState.AddModelError("Body", "The body exceeds the required length");

            //ACT
            var result = _controller.Create(It.IsAny<ReviewModel>()).Result;

            //ASSERT
            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }

        [Test]
        public void Put_ExistingReviewIsSavedInRepository()
        {
            //ACT
            var result = _controller.Update(_newReview.Id, _reviewModel).Result as OkResult;

            //ASSERT
            _reviewRepositoryMock.Verify(repository => repository.UpdateAsync(It.IsAny<Review>()), Times.Once);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<OkResult>());
        }

        [Test]
        public void Put_InValidReviewModelStateCausesBadRequest()
        {
            //ARRANGE
            _controller.ModelState.AddModelError("Body", "The body exceeds the required length");

            //ACT
            var result = _controller.Update(It.IsAny<int>(), It.IsAny<ReviewModel>()).Result;

            //ASSERT
            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }

        [Test]
        public void Delete_ExistingReviewIsDeletedFromRepository()
        {
            //ASSERT
            _reviewRepositoryMock.Setup(repository => repository.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(_newReview);

            //ACT
            var result = _controller.Delete(_newReview.Id).Result as OkResult;

            //ASSERT
            _reviewRepositoryMock.Verify(repository => repository.DeleteAsync(It.IsAny<int>()), Times.Once);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<OkResult>());
        }

        [Test]
        public void Delete_NonExistingReviewReturnsNotFound()
        {
            //ACT
            var result = _controller.Delete(It.IsAny<int>()).Result as NotFoundResult;

            //ASSERT
            _reviewRepositoryMock.Verify(repository => repository.GetByIdAsync(It.IsAny<int>()), Times.Once);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }
    }
}
