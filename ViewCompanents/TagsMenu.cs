
using BlogApps.Data.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApps.ViewCompanents
{
    
    public class TagsMenu:ViewComponent
    {
        private ITagRepository _Tagrepostitory;

        public TagsMenu(ITagRepository Tagrepostitory)
        {
            _Tagrepostitory = Tagrepostitory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _Tagrepostitory.Tags.ToListAsync());
        }
    }
}