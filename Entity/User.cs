using System.Net.Mail;
using Microsoft.AspNetCore.Identity;

namespace BlogApps.Entity
{
    public class User : IdentityUser
    {
        public string? FullName { get; set; }
        public string? Image { get; set; }

        public string? AboutMe { get; set; }
        public string? Linkedin { get; set; }

        public bool Copyright { get; set; }
        public List<Post> posts { get; set; } = new List<Post>();
        public List<LikePosts> LikePost { get; set; }= new List<LikePosts>();
        public List<Comment> comments { get; set; } = new List<Comment>();

    }
}
