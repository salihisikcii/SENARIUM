using BlogApps.Entity;
using Microsoft.EntityFrameworkCore;

namespace BlogApps.Data.Abstract
{
    public interface IPostRequestRepository
    {
        DbSet<PostRequest> PostRequests { get; }  // DbSet olarak değiştirildi

        void CreatePostRequest(PostRequest postRequest);
        void Update(PostRequest postRequest);  
        Task SaveAsync();  
    }
}
