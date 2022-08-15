using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OdeToFood.Api.Services;
using OdeToFood.Data.DomainClasses;
using OdeToFood.Data.ViewModels;

namespace OdeToFood.Api.Controllers.Api
{
    [Route("")]
    public class HomeController : Controller
    {
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IReviewFactory _reviewFactory;

        public HomeController(IRestaurantRepository restaurantRepository, IReviewRepository reviewRepository, IReviewFactory reviewFactory)
        {
            _restaurantRepository = restaurantRepository;
            _reviewRepository = reviewRepository;
            _reviewFactory = reviewFactory;
        }

        [Route("")]
        public IActionResult Index()
        {
            var model = _restaurantRepository.GetAll();

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [Route("[Controller]/[action]/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var restaurant = _restaurantRepository.GetById(id);

            if (restaurant == null)
            {
                return RedirectToAction(nameof(Index));
            }

            IList<Review> reviews = await _reviewRepository.GetAllAsync();
            IEnumerable<Review> restaurantReviews = reviews.Where(review => review.RestaurantId == restaurant.Id);

            RestaurantDetails details = new RestaurantDetails
            {
                Restaurant = restaurant,
                Reviews = restaurantReviews
            };

            var model = details;


            return View(model);
        }

        [Route("[controller]/[action]/{id}")]
        public IActionResult AddReview(int id, ReviewModel reviewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            Restaurant restaurant = _restaurantRepository.GetById(id);
            Review newReview = _reviewFactory.Create(restaurant, reviewModel);

            _reviewRepository.AddAsync(newReview);

            return RedirectToAction(nameof(Details), new { id = restaurant.Id }); ;
        }
    }
}