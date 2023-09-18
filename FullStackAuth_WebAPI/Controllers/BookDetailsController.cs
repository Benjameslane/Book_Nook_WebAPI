using FullStackAuth_WebAPI.Data;
using FullStackAuth_WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;

namespace FullStackAuth_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookDetailsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BookDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{bookId}")]
        public IActionResult Get(string bookId)
        {
            try
            {
                // Get the userId from the logged-in user.
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // Retrieve all reviews for the provided bookId, including the related user data.
                var reviews = _context.Reviews
                    .Where(r => r.BookId == bookId)
                    .Select(r => new ReviewWithUserDto
                    {
                        Text = r.Text,
                        Rating = r.Rating,
                        Username = r.User.FirstName + " " + r.User.LastName // Assuming you want full name here.
                    })
                    .ToList();

                // Calculate the average rating.
                double? averageRating = reviews.Any() ? reviews.Average(r => r.Rating) : (double?)null;

                // Check if the logged-in user has favorited this book.
                bool isFavorited = _context.Favorites.Any(f => f.BookId == bookId && f.UserId == userId);

                // Construct the final DTO.
                var bookDetailsDto = new BookDetailsDto
                {
                    Reviews = reviews,
                    AverageRating = averageRating,
                    IsFavorited = isFavorited
                };

                return Ok(bookDetailsDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }

    public class ReviewWithUserDto
    {
        public string Text { get; set; }
        public double Rating { get; set; }
        public string Username { get; set; }
    }

    public class BookDetailsDto
    {
        public List<ReviewWithUserDto> Reviews { get; set; }
        public double? AverageRating { get; set; }
        public bool IsFavorited { get; set; }
    }
}
