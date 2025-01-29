using BlogApps.Entity;

namespace BlogApps.Data.Abstract
{
    public interface IPostRepository
    {
        IQueryable<Post> Posts { get; }

        void CreatePost(Post post);
        void Update(Post post);
        Task SaveChangesAsync();


    }
}
