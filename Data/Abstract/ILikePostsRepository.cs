using BlogApps.Entity;

namespace BlogApps.Data.Abstract
{
    public interface ILikePostsRepository
    {
        IQueryable<LikePosts> LikePosts { get; }

        void CreateLikePosts(LikePosts likePosts);
        void Update(LikePosts likePosts);  
        Task SaveAsync();  
    }
}
