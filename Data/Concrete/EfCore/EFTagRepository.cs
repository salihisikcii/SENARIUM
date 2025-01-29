using BlogApps.Entity;
using BlogApps.Data.Abstract;
using Microsoft.EntityFrameworkCore;

namespace BlogApps.Data.Concrete.EfCore
{
    public class EFTagRepository : ITagRepository
    {
        private BlogContext _context;

        public EFTagRepository(BlogContext context)
        {
            _context = context;
        }
        public IQueryable<Tag> Tags => _context.Tags;
        public async Task<List<Tag>> GetTagsAsync()
        {
            return await _context.Tags.ToListAsync();
        }
        public async Task<Tag> GetTagAsync(int tagId)
        {
            return await _context.Tags.FindAsync(tagId)?? new();
        }
        public void CreateTag(Tag Tag)
        {
            _context.Tags.Add(Tag);
            _context.SaveChanges();
        }
    }
}
