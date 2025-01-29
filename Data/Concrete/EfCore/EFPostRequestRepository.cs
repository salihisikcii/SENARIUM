
using BlogApps.Data.Abstract;
using BlogApps.Entity;
using Microsoft.EntityFrameworkCore;

namespace BlogApps.Data.Concrete.EfCore
{
    public class EFPostRequestRepository : IPostRequestRepository
    {

        private BlogContext _context;

        public EFPostRequestRepository(BlogContext context)
        {
            _context = context;
        }



        DbSet<PostRequest> IPostRequestRepository.PostRequests => _context.PostRequests;

        public void CreatePostRequest(PostRequest postRequest)
        {
            _context.PostRequests.Add(postRequest);
        }
        public void Update(PostRequest postRequest)
        {
            _context.PostRequests.Update(postRequest);
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

