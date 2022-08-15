using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OdeToFood.Data.DomainClasses;

namespace OdeToFood.Api.Services
{
    public interface IRestaurantRepository
    {
        List<Restaurant> GetAll();
        Restaurant Add(Restaurant restaurant);
        Restaurant GetById(int id);
        void Update(Restaurant restaurant);
        void Delete(int id);
    }
}
