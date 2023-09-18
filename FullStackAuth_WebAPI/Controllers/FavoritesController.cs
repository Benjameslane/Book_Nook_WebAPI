using FullStackAuth_WebAPI.Data;
using FullStackAuth_WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Linq;
using System.Net;



namespace FullStackAuth_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoritesController : ControllerBase

    {

        private readonly ApplicationDbContext _context;

        public FavoritesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost, Authorize]
        public IActionResult Post([FromBody] Favorite data)
        {
            try
            {
                // Retrieve the authenticated user's ID from the JWT token
                string userId = User.FindFirstValue("id");

                // If the user ID is null or empty, the user is not authenticated, so return a 401 unauthorized response
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                // Set the car's owner ID  the authenticated user's ID we found earlier
                data.UserId = userId;

                // Add the car to the database and save changes
                _context.Favorites.Add(data);
                if (!ModelState.IsValid)
                {
                    // If the car model state is invalid, return a 400 bad request response with the model state errors
                    return BadRequest(ModelState);
                }
                _context.SaveChanges();

                // Return the newly created car as a 201 created response
                return StatusCode(201, data);
            }
            catch (Exception ex)
            {
                // If an error occurs, return a 500 internal server error with the error message
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("myFavorites"), Authorize]
        public IActionResult GetUsersFavorites()
        {
            try
            {
                // Retrieve the authenticated user's ID from the JWT token
                string userId = User.FindFirstValue("id");

                // Retrieve all cars that belong to the authenticated user, including the owner object
                var cars = _context.Favorites.Where(c => c.UserId.Equals(userId));

                // Return the list of cars as a 200 OK response
                return Ok(cars);

            }
            catch (Exception ex)
            {
                // If an error occurs, return a 500 internal server error with the error message
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);

            }
        }
    }
}