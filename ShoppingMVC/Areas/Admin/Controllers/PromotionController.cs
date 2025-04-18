using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shopping.Utility;
using ShoppingMVC.DataAccess.Data;
using ShoppingMVC.DataAccess.Repository.IRepository;
using ShoppingMVC.Models;
using ShoppingMVC.Models.ViewModels;
using System.Collections.Generic;
using System.Data;

namespace ShoppingMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class PromotionController : Controller    {
        
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public PromotionController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Promotion> objPromotionList = _unitOfWork.Promotion.GetAll().ToList();
            
            return View(objPromotionList);
        }
               

        public IActionResult UpdateInsert(int? id)
        {
            PromotionVM promotionVM = new()
            {
                PromotionList = _unitOfWork.Promotion.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Title,
                    Value = u.Id.ToString()

                }),
                Promotion = new Promotion()
            };

            if (id == null || id == 0)
            {
                // Create
                return View(promotionVM);
            }

            else
            {
                // Update
                promotionVM.Promotion = _unitOfWork.Promotion.Get(u=>u.Id==id);
                return View(promotionVM);
            }
        }

        [HttpPost]
        public IActionResult UpdateInsert(PromotionVM promotionVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    // Assign random name to a file
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string promotionPath = Path.Combine(wwwRootPath, @"images\promotion");

                    if (!string.IsNullOrEmpty(promotionVM.Promotion.ImageUrl)) 
                    {
                        // Delete old image
                        var oldImagePath = Path.Combine(wwwRootPath, promotionVM.Promotion.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(promotionPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    promotionVM.Promotion.ImageUrl = @"\images\promotion\"+ fileName;
                }

                if(promotionVM.Promotion.Id == 0)
                {
                    _unitOfWork.Promotion.Add(promotionVM.Promotion);
                }
                else
                {
                    _unitOfWork.Promotion.Update(promotionVM.Promotion);
                }

                _unitOfWork.Save();
                TempData["success"] = "Promotion created successfully";
                return RedirectToAction("Index");
            }

            else
            {
                promotionVM.PromotionList = _unitOfWork.Promotion.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Title,
                    Value = u.Id.ToString()

                });

                return View(promotionVM);
            }
        }
     

       #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Promotion> objPromotionList = _unitOfWork.Promotion.GetAll().ToList();
            return Json(new { data = objPromotionList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var promoToDelete = _unitOfWork.Promotion.Get(u => u.Id == id);
            if (promoToDelete == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, promoToDelete.ImageUrl.TrimStart('\\'));

            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            // Remove the promotion to be deleted
            _unitOfWork.Promotion.Delete(promoToDelete);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Successfully deleted!" });
        }
        #endregion

    }
}
