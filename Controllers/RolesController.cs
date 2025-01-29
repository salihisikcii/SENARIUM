using BlogApps.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // To use ToListAsync

namespace BlogApps.Controllers
{
    public class RolesController : Controller
    {
        private readonly RoleManager<UserRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<RolesController> _logger;

        public RolesController(RoleManager<UserRole> roleManager, UserManager<User> userManager, ILogger<RolesController> logger)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Index metoduna erişildi.");
            var roles = await _roleManager.Roles.ToListAsync();
            var roleViewModels = new List<RoleViewModel>();

            foreach(var role in roles)
            {
              
                if(role.Name != null)
                {
                var users = await _userManager.GetUsersInRoleAsync(role.Name);
                roleViewModels.Add(new RoleViewModel
                {
                    Id = role.Id,
                    Name = role.Name,
                    Users = users.Select(u => u.UserName).ToList()!
                });
                }
                
            }

            return View(roleViewModels);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserRole model)
        {
            if(ModelState.IsValid)
            {
                var result = await _roleManager.CreateAsync(model);

                if(result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }

            }
            return View(model);
        }
    
        public async Task<IActionResult> Edit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if(role != null && role.Name != null)
            {
                ViewBag.Users = await _userManager.GetUsersInRoleAsync(role.Name);
                return View(role);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserRole model)
        {
            if(ModelState.IsValid)
            {
                var role = await _roleManager.FindByIdAsync(model.Id);

                if(role != null)
                {
                    role.Name = model.Name;

                    var result = await _roleManager.UpdateAsync(role);

                    if(result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }

                    foreach (var err in result.Errors)
                    {
                        ModelState.AddModelError("", err.Description);
                    }

                    if(role.Name != null)
                        ViewBag.Users = await _userManager.GetUsersInRoleAsync(role.Name);
                }
            }

            return View(model);
        }
    }   
}
