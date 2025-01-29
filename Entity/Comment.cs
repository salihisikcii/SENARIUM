using System.ComponentModel.DataAnnotations;

namespace BlogApps.Entity
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }  

        public string? Text { get; set; }

        public DateTime PublishedOn { get; set; }

        public int PostId { get; set; }
        public Post post { get; set; } = null!;

        public string? UserId { get; set; }
        public User user { get; set; } = null!;
    }
}
