using System.ComponentModel.DataAnnotations;

namespace BlogApps.Entity
{
    public class LikePosts
    {
        [Key]
        public int LikedPostId { get; set; }  

        public bool kontrol { get; set; }        
        public int PostId { get; set; }

        public Post? Post { get; set; }
        
        public string? UserId { get; set; }

        public User? user { get; set; } 
    }
}
