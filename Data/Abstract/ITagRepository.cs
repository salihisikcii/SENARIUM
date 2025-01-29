using BlogApps.Entity;

namespace BlogApps.Data.Abstract
{
    public interface ITagRepository
    {
        IQueryable<Tag> Tags { get; }

        void CreateTag(Tag tag);
        Task<List<Tag>> GetTagsAsync();
        Task<Tag> GetTagAsync(int tagId); // Belirli bir tag'i getirir

    }
}
