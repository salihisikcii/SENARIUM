using BlogApps.Entity;

namespace BlogApps.Data.Abstract
{
    public interface IUserRepository
    {
        IQueryable<User> Users { get; }

        void CreateUser(User user);
        Task SaveChangesAsync();

    }
}
