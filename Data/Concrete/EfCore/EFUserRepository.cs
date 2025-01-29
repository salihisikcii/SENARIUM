
using BlogApps.Data.Abstract;
using BlogApps.Entity;

namespace BlogApps.Data.Concrete.EfCore
{
    public class EFUserRepository : IUserRepository
    {
        private  BlogContext _context;

        public EFUserRepository(BlogContext context)
        {
            _context= context;
        }
        public IQueryable<User> Users => _context.Users;

        public void CreateUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }
          public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}