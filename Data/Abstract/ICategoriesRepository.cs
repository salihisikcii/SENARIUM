using BlogApps.Entity;

namespace BlogApps.Data.Abstract
{
    public interface ICategoriesRepository
    {
        IQueryable<Categories> Categories { get; }

        Task<List<Categories>> GetCategoriesAsync(); // Tüm kategorileri getirir


        void CreateCategories(Categories categories);
    }
}
