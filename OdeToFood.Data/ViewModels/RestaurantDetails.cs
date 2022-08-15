using System;
using System.Collections.Generic;
using System.Text;
using OdeToFood.Data.DomainClasses;

namespace OdeToFood.Data.ViewModels
{
    public class RestaurantDetails
    {
        public Restaurant Restaurant { get; set; }
        public IEnumerable<Review> Reviews { get; set; }
    }
}
