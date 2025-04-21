using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shopping.Utility;
using ShoppingMVC.DataAccess.Data;
using ShoppingMVC.DataAccess.Repository.IRepository;
using ShoppingMVC.Models;
using ShoppingMVC.Models.ViewModels;

namespace ShoppingMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ApplicationUserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public ApplicationUserController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) //, IWebHostEnvironment webHostEnvironment
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
            //_webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index(string? searchTerm, string? roleFilter)
        {
            var users = _userManager.Users.ToList();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                users = users.Where(u =>
                    (!string.IsNullOrEmpty(u.Name) && u.Name.ToLower().Contains(searchTerm)) ||
                    (!string.IsNullOrEmpty(u.UserName) && u.UserName.ToLower().Contains(searchTerm))
                ).ToList();
            }

            var userWithRoles = new List<ApplicationUserWithRoleVM>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var role = roles.FirstOrDefault() ?? "No Role";

                // Filter by role if needed
                if (!string.IsNullOrEmpty(roleFilter) && role != roleFilter)
                    continue;

                userWithRoles.Add(new ApplicationUserWithRoleVM
                {
                    User = user,
                    Role = role
                });
            }

            var allRoles = _roleManager.Roles.Select(r => new SelectListItem
            {
                Value = r.Name,
                Text = r.Name
            }).ToList();

            var viewModel = new UserVM
            {
                Users = userWithRoles,
                Roles = allRoles,
                RoleFilter = roleFilter
            };

            return View(viewModel);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ApplicationUser obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.ApplicationUser.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "User created successfully";
                return RedirectToAction("Index", "ApplicationUser");
            }
            return View();
        }

        public IActionResult Edit(string? id)
        {
            ApplicationUser? userfromDb = _unitOfWork.ApplicationUser.Get(u => u.Id == id);

            if (userfromDb == null)
            {
                return NotFound();
            }
            return View(userfromDb);
        }

        [HttpPost]
        public IActionResult Edit(ApplicationUser obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.ApplicationUser.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "User updated successfully";
                return RedirectToAction("Index", "ApplicationUser");
            }
            return View();
        }

        public IActionResult Delete(string? id)
        {
            ApplicationUser? userfromDb = _unitOfWork.ApplicationUser.Get(u => u.Id == id);

            if (userfromDb == null)
            {
                return NotFound();
            }
            return View(userfromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(string? id)
        {
            ApplicationUser obj =  _unitOfWork.ApplicationUser.Get(u => u.Id ==id);
            if (obj == null)
            {
                return NotFound();
            }

            _unitOfWork.ApplicationUser.Delete(obj);
            _unitOfWork.Save();
            TempData["success"] = "User deleted successfully";
            return RedirectToAction("Index", "ApplicationUser");

        }


    }
}
