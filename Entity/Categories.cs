using System.ComponentModel.DataAnnotations;

namespace BlogApps.Entity
{

    public class Categories
    {
        [Key]
        public int CategoryId { get; set; }

        public string? Text { get; set; }
        public string? Url { get; set; }

        public List<Post> posts { get; set; } = new List<Post>();
    }
}
