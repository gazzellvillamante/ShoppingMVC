using Microsoft.AspNetCore.Authorization;
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
        public ApplicationUserController(IUnitOfWork unitOfWork) //, IWebHostEnvironment webHostEnvironment
        {
            _unitOfWork = unitOfWork;
            //_webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index(string? searchTerm)
        {
            /**List<ApplicationUser> objUserList = _unitOfWork.ApplicationUser.GetAll().ToList();
            return View(objUserList);**/

            IEnumerable<ApplicationUser> objUserList = _unitOfWork.ApplicationUser.GetAll();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                objUserList = objUserList.Where(u =>
                    (!string.IsNullOrEmpty(u.Name) && u.Name.ToLower().Contains(searchTerm)) ||
                    (!string.IsNullOrEmpty(u.UserName) && u.UserName.ToLower().Contains(searchTerm))
                );
            }

            return View(objUserList.ToList());
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
