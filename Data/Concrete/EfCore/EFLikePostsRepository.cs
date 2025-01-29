using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApps.Data.Abstract;
using BlogApps.Entity;

namespace BlogApps.Data.Concrete.EfCore
{
    public class EFLikePostsRepository : ILikePostsRepository
    {

        private BlogContext _context;

        public EFLikePostsRepository(BlogContext context)
        {
            _context = context;
        }


        public IQueryable<LikePosts> LikePosts => _context.LikePosts;


        public void CreateLikePosts(LikePosts likePosts)
        {
            _context.LikePosts.Add(likePosts);
        }
        public void Update(LikePosts likePosts)
        {
            _context.LikePosts.Update(likePosts);
        }


        public async Task SaveAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Hata loglaması yap
                throw new Exception("Veritabanı kaydedilemedi: " + ex.Message, ex);
            }
        }

    }
}

