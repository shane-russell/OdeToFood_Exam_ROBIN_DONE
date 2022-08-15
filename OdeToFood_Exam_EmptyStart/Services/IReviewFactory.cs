using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OdeToFood.Data.DomainClasses;

namespace OdeToFood.Api.Services
{
    public interface IReviewFactory
    {
        Review Create(Restaurant restaurant, ReviewModel reviewModel);
    }
}
