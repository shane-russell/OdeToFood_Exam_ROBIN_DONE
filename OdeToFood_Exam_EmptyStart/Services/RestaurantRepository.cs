using System.Collections.Generic;
using System.Linq;
using OdeToFood.Data;
using OdeToFood.Data.DomainClasses;

namespace OdeToFood.Api.Services
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly OdeToFoodContext _context;

        public RestaurantRepository(OdeToFoodContext context)
        {
            _context = context;
        }

        public List<Restaurant> GetAll()
        {
            return _context.Restaurants.ToList();
        }

        public Restaurant GetById(int id)
        {
            return _context.Restaurants.FirstOrDefault(r => r.Id == id);
        }

        public Restaurant Add(Restaurant restaurant)
        {
            _context.Restaurants.Add(restaurant);
            _context.SaveChanges();

            return restaurant;
        }

        public void Update(Restaurant restaurant)
        {
            var original = _context.Restaurants.Find(restaurant.Id);

            var entry = _context.Entry(original);

            entry.CurrentValues.SetValues(restaurant);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var restaurant = GetById(id);

            _context.Restaurants.Remove(restaurant);
            _context.SaveChanges();
        }
    }
}
