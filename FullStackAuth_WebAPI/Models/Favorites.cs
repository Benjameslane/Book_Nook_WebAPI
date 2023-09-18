using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FullStackAuth_WebAPI.Models
{
    public class Favorite
    {
        [Key]
        
        public int Id { get; set; }

      
        public string BookId { get; set; }

      
        public string Title { get; set; }

      
        public string ThumbnailUrl { get; set; }

       
        public string UserId { get; set; }

        // Navigation property for the related User entity.
        public virtual User User { get; set; }
    }
}
