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
        private readonly ILogger<ApplicationUserController> _logger;
        public ApplicationUserController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ILogger<ApplicationUserController> logger) //, IWebHostEnvironment webHostEnvironment
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
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
            var roles = _roleManager.Roles
                .Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Name
                });

            var model = new CreateUpdateUserViewModel
            {
                RoleList = roles
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUpdateUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        _logger.LogWarning("Validation error in {Field}: {Error}", state.Key, error.ErrorMessage);
                    }
                }

                // Re-populate RoleList before returning view
                model.RoleList = _roleManager.Roles.Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Name
                });

                return View(model);
            }

            var newUser = new ApplicationUser
            {
                UserName = model.UserName,
                Name = model.Name,
                Street = model.Street,
                City = model.City,
                Suburb = model.Suburb,
                PostCode = model.PostCode
            };

            var result = await _userManager.CreateAsync(newUser, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, model.Role);

                TempData["success"] = "User created successfully";
                return RedirectToAction("Index");
            }

            // 🔍 Identity creation errors
            foreach (var error in result.Errors)
            {
                Console.WriteLine($"Identity Error: {error.Code} - {error.Description}");
                ModelState.AddModelError(string.Empty, error.Description);
            }

            model.RoleList = _roleManager.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Name
            });

            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            var roles = await _userManager.GetRolesAsync(user);
            var currentRole = roles.FirstOrDefault();

            var model = new CreateUpdateUserViewModel
            {
                Name = user.Name,
                UserName = user.UserName,
                Street = user.Street,
                City = user.City,
                Suburb = user.Suburb,
                PostCode = user.PostCode,
                Role = currentRole ?? "",
                RoleList = _roleManager.Roles.Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Name
                })
            };



            return View("Edit", model); // You can reuse the Create view or make a copy named Edit.cshtml
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, CreateUpdateUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.RoleList = _roleManager.Roles.Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Name
                });
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            // Update user properties
            user.Name = model.Name;
            user.UserName = model.UserName;
            user.Street = model.Street;
            user.City = model.City;
            user.Suburb = model.Suburb;
            user.PostCode = model.PostCode;

            var result = await _userManager.UpdateAsync(user);

            if (!string.IsNullOrEmpty(model.Role))
            {
                var currentRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
                await _userManager.AddToRoleAsync(user, model.Role);
            }

            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                // Remove old password (if set), then reset it
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var resetPassResult = await _userManager.ResetPasswordAsync(user, token, model.Password);

                if (!resetPassResult.Succeeded)
                {
                    foreach (var error in resetPassResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    model.RoleList = _roleManager.Roles.Select(r => new SelectListItem
                    {
                        Text = r.Name,
                        Value = r.Name
                    });

                    return View(model);
                }
            }

            if (result.Succeeded)
            {
                TempData["success"] = "User updated successfully";
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            model.RoleList = _roleManager.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Name
            });

            return View(model);
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
