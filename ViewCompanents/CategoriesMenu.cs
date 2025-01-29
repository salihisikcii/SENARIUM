using BlogApps.Data.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApps.ViewCompanents
{

    public class CategoiresMenu : ViewComponent
    {
        private ICategoriesRepository _Categoriesrepostitory;

        public CategoiresMenu(ICategoriesRepository Categories)
        {
            _Categoriesrepostitory = Categories;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _Categoriesrepostitory.Categories.ToListAsync());
        }
    }
}