namespace FullStackAuth_WebAPI.DataTransferObjects
{
    using System.Collections.Generic;

    public class BookDetailsDto
    {
        public string BookId { get; set; }
        public List<ReviewWithUserDto> Reviews { get; set; }
        public double AverageRating { get; set; }
        public bool IsFavoritedByUser { get; set; }
    }

}