using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OdeToFood.Data.DomainClasses;

namespace OdeToFood.Api.Services
{
    public class ReviewFactory : IReviewFactory
    {

        public Review Create(Restaurant restaurant, ReviewModel reviewModel)
        {
            Review newReview = new Review
            {
                Body = reviewModel.Body,
                Rating = reviewModel.Rating,
                Restaurant = restaurant,
                RestaurantId = restaurant.Id,
                ReviewerName = reviewModel.ReviewerName
            };

            return newReview;
        }
    }
}
