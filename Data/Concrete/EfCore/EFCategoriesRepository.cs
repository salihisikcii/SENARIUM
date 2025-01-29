using BlogApps.Entity;
using BlogApps.Data.Abstract;
using Microsoft.EntityFrameworkCore;

namespace BlogApps.Data.Concrete.EfCore
{
    public class EFCategoriesRepository : ICategoriesRepository
    {
        private BlogContext _context;

        public EFCategoriesRepository(BlogContext context)
        {
            _context = context;
        }
        public async Task<List<Categories>> GetCategoriesAsync()
        {
            return await _context.Categories.ToListAsync(); // Tüm kategorileri getir
        }
        public IQueryable<Categories> Categories => _context.Categories;

        public void CreateCategories(Categories categories)
        {
            _context.Categories.Add(categories);
            _context.SaveChanges();
        }
    }
}
