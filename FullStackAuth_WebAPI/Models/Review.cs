using System.ComponentModel.DataAnnotations.Schema;

namespace FullStackAuth_WebAPI.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string BookId { get; set; }
        public string Text { get; set; }
        public double Rating { get; set; }
       

        // This is a navigation property. It denotes the relationship between Review and User.
        // This assumes you have a User model class defined in your project.

        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
