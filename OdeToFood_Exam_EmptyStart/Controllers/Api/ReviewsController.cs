using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OdeToFood.Api.Services;
using OdeToFood.Data.DomainClasses;

namespace OdeToFood.Api.Controllers.Api
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewRepository _reviewRepository;

        public ReviewsController(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var model = await _reviewRepository.GetAllAsync();
            if (model == null)
            {
                return NotFound();
            }
            return Ok(model);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var model = await _reviewRepository.GetByIdAsync(id);

            if (model == null)
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] ReviewModel reviewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            Review review = new Review
            {
                Body = reviewModel.Body,
                Rating = reviewModel.Rating,
                Restaurant = reviewModel.Restaurant,
                RestaurantId = reviewModel.RestaurantId,
                ReviewerName = reviewModel.ReviewerName
            };

            await _reviewRepository.AddAsync(review);

            return CreatedAtAction(nameof(GetById), new { id = review.Id }, review);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id,[FromBody] ReviewModel reviewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            Review review = new Review
            {
                Id = id,
                Body = reviewModel.Body,
                Rating = reviewModel.Rating,
                Restaurant = reviewModel.Restaurant,
                RestaurantId = reviewModel.RestaurantId,
                ReviewerName = reviewModel.ReviewerName
            };
            await _reviewRepository.UpdateAsync(review);
            return Ok();
        }

        [HttpDelete ("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            if (await _reviewRepository.GetByIdAsync(id) == null)
            {
                return NotFound();
            }

            await _reviewRepository.DeleteAsync(id);

            return Ok();
        }
    }
}