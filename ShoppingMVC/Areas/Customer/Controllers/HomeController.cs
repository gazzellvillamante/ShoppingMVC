using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
using ShoppingMVC.DataAccess.Repository;
using ShoppingMVC.DataAccess.Repository.IRepository;
using ShoppingMVC.Models;
using System.Diagnostics;

namespace ShoppingMVC.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHomeRepository _homeRepository;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork, IHomeRepository homeRepository)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _homeRepository = homeRepository;
        }

        public async Task<IActionResult> Index(string searchQuery = "", int categoryId = 0)
        {
            IEnumerable<Product> producList = await _homeRepository.GetProducts(searchQuery, categoryId);
            IEnumerable<Category> categories = await _homeRepository.Categories();
            ProductDisplay productModel = new ProductDisplay
            {
                Products = producList,
                Categories = categories
            };


            return View(productModel);
        }

        
        public IActionResult Detail(int productId)
        {
            Product product = _unitOfWork.Product.Get(u=>u.Id== productId, includeProperties: "Category");
            return View(product);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
