
using System.ComponentModel;
using BlogApps.Data.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApps.ViewCompanents
{
    public class Popular : ViewComponent
    {
        private IPostRepository _Postrepostitory;
        public Popular(IPostRepository Postrepostitory)
        {
            _Postrepostitory = Postrepostitory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var posts = await _Postrepostitory
                .Posts
                .Include(p => p.User)
                .Where(p => p.IsBest)
                .OrderByDescending(p => p.Vote)
                .Take(5)
                .ToListAsync();

            return View(posts);
        }
    }
}