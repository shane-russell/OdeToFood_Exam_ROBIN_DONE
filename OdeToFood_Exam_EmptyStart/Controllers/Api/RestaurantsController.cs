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
    public class RestaurantsController : ControllerBase
    {
        private readonly IRestaurantRepository _restaurantRepository;

        public RestaurantsController(IRestaurantRepository restaurantRepository)
        {
            _restaurantRepository = restaurantRepository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            var model = _restaurantRepository.GetAll();

            if (model == null)
            {
                return NotFound();
            }
            return Ok(model);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(int id)
        {
            var model = _restaurantRepository.GetById(id);

            if (model == null)
            {
                return NotFound();
            }
            return Ok(model);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Create([FromBody] RestaurantModel restaurantModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            Restaurant restaurant = new Restaurant
            {
                City = restaurantModel.City,
                Country = restaurantModel.Country,
                Name = restaurantModel.Name
            };
            _restaurantRepository.Add(restaurant);
            return CreatedAtAction(nameof(GetById), new { id = restaurant.Id }, restaurant);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Update(int id, [FromBody] RestaurantModel restaurantModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            Restaurant restaurant = new Restaurant
            {
                Id = id,
                City = restaurantModel.City,
                Country = restaurantModel.Country,
                Name = restaurantModel.Name
            };
            _restaurantRepository.Update(restaurant);
            return Ok();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Delete(int id)
        {
            if (_restaurantRepository.GetById(id) == null)
            {
                return NotFound();
            }

            _restaurantRepository.Delete(id);
            return Ok();
        }
    }
}