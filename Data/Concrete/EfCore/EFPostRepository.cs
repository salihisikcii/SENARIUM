using BlogApps.Entity;
using BlogApps.Data.Abstract;
using Microsoft.EntityFrameworkCore;

namespace BlogApps.Data.Concrete.EfCore
{
    public class EFPostRepository : IPostRepository
    {
        private BlogContext _context;

        public EFPostRepository(BlogContext context)
        {
            _context = context;
        }
        public IQueryable<Post> Posts => _context.Posts;

        public void CreatePost(Post post)
        {
            // Kullanıcı ID'sinin geçerliliğini kontrol et
            if (_context.Users.Any(u => u.Id == post.UserId))
            {
                _context.Posts.Add(post);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Geçersiz kullanıcı ID.");
            }
        }
        public void Update(Post post)
        {
            _context.Update(post);

        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
