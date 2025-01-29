using System.ComponentModel.DataAnnotations;

namespace BlogApps.Entity
{
    public class Post
    {
        [Key]
        public int PostId { get; set; }

        public string? Title { get; set; }
        public string? Description { get; set; } // özet 

        public string? Content { get; set; } //pdf yükleme
        public string? Url { get; set; }

        public string? Image { get; set; }

        public DateTime PublishedOn { get; set; }
        
        public string? SHA256Hash { get; set; }


        public bool IsActive { get; set; }
        public bool IsRequestRequired  { get; set; }

        public int LikedPostId { get; set; }

        public LikePosts? LikePost { get; set; }
        public string? UserId { get; set; }
        public User? User { get; set; }
        public int Vote { get; set; }

        public List<Tag> tags { get; set; } = new();
        public List<Categories> categories { get; set; } = new();

        public int Price { get; set; }
        public List<Comment> comments { get; set; } = new();

        public bool IsBest { get; set; }
        public bool PdfActive { get; set; }


    }
}
