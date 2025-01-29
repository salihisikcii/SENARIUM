using BlogApps.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlogApps.Data.Concrete.EfCore
{
    public class BlogContext : IdentityDbContext<User, UserRole, string>
    {
        public BlogContext(DbContextOptions<BlogContext> options) : base(options)
        {
        }

        public DbSet<Post> Posts => Set<Post>();
        public DbSet<Tag> Tags => Set<Tag>();
        public DbSet<Categories> Categories => Set<Categories>();
        public DbSet<Comment> Comments => Set<Comment>();
        public DbSet<LikePosts> LikePosts => Set<LikePosts>();
        public DbSet<PostRequest> PostRequests => Set<PostRequest>();

        // Başlıkla arama yapan fonksiyon
        public IQueryable<Post> SearchPostsByTitle(string searchTerm)
        {
            return Posts.Where(p => p.Title != null && p.Title.Contains(searchTerm)); // Null olmayan başlıklar için arama
        }

    }
}
