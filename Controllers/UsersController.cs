
using BlogApps.Entity;
using BlogApps.UserModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BlogApps.Controllers
{
    public class UsersController : Controller
    {
        private UserManager<User> _userManager;
        private RoleManager<UserRole> _roleManager;
        public UsersController(UserManager<User> userManager, RoleManager<UserRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        [Authorize(Roles = "Admin")]

        public IActionResult UserList()
        {
            return View(_userManager.Users);
        }

        public async Task<IActionResult> ProfileEdit(string url)
        {
            if (url == null)
            {
                return RedirectToAction("Index");
            }

            var user = await _userManager.FindByNameAsync(url);

            if (user != null)
            {
                ViewBag.Roles = await _roleManager.Roles.Select(i => i.Name).ToListAsync();

              var teuser =  new EditViewModel
                {
                    UserId = user.Id,
                    FullName = user.FullName ?? "",
                    Email = user.Email ?? "",
                    UserName = user.UserName ?? "",
                    image = user.Image ?? "",
                    AboutMe = user.AboutMe ?? "",
                    Linkedin = user.Linkedin ?? "",

                };

                return View(teuser);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ProfileEdit(EditViewModel model, IFormFile image)
        {



            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user != null)
            {
                user.Email = model.Email;
                user.FullName = model.FullName;
                user.AboutMe = model.AboutMe;
                user.Linkedin = model.Linkedin;
                user.UserName = model.UserName;

                if (image != null && image.Length > 0)
                {
                    string imageFileName = GenerateUniqueFileName(image.FileName);
                    string imagePath = Path.Combine("wwwroot/img", imageFileName);

                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    user.Image = imageFileName;
                }

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded && !string.IsNullOrEmpty(model.Password))
                {
                    await _userManager.RemovePasswordAsync(user);
                    await _userManager.AddPasswordAsync(user, model.Password);
                }

                if (result.Succeeded)
                {
                    await _userManager.RemoveFromRolesAsync(user, await _userManager.GetRolesAsync(user));
                    if (model.SelectedRoles != null)
                    {
                        await _userManager.AddToRolesAsync(user, model.SelectedRoles);
                    }
                    return  RedirectToAction("Profile", "Users", new { userName = user.UserName });;
                }

                foreach (IdentityError err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
            }


            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Profile(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return NotFound();
            }

            var user = await _userManager
                        .Users
                        .Include(x => x.posts)
                        .Include(x => x.comments)
                        .ThenInclude(x => x.post)
                        .FirstOrDefaultAsync(x => x.UserName == username);

            if (user == null)
            {
                return NotFound();
            }
            var roles = await _userManager.GetRolesAsync(user);
            ViewBag.UserRole = roles.FirstOrDefault();

            return View(user);
        }

        private string GenerateUniqueFileName(string fileName)
        {
            var fileExtension = Path.GetExtension(fileName);
            var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
            return uniqueFileName;
        }

    }


}