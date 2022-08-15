using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OdeToFood.Data;
using OdeToFood.Data.DomainClasses;

namespace OdeToFood.Api.Services
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly OdeToFoodContext _context;

        public ReviewRepository(OdeToFoodContext context)
        {
            _context = context;
        }

        public async Task<IList<Review>> GetAllAsync()
        {
            return await _context.Reviews.ToListAsync();
        }

        public async Task<Review> GetByIdAsync(int id)
        {
            return await _context.Reviews.FirstOrDefaultAsync(review => review.Id == id);
        }

        public async Task<Review> AddAsync(Review review)
        {
            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task UpdateAsync(Review review)
        {
            var original = _context.Reviews.Find(review.Id);

            var entry = _context.Entry(original);

            entry.CurrentValues.SetValues(review);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var review = await GetByIdAsync(id);

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
        }
    }
}
