using ImazhMenu.Models;
using ImazhMenu.Repository.IRepository;
using ImazhMenu.ViewModels.Account;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
using System.Diagnostics;

namespace ImazhMenu.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;

        }

        public IActionResult Index()
        {
            var categories = _unitOfWork.Category.GetAllCategories();
            var products = _unitOfWork.SubCategory.GetAllSubCategories();
            var gallerry = _unitOfWork.Gallery.GetAllGalleryPictures();
            AdminPanelViewModel model = new AdminPanelViewModel()
            {
                Categories = categories,
                SubCategories = products,
                Gallery = gallerry
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult GetProductsByCategoryId()
        {
            var CatId = Request.Form["CategoryId"].FirstOrDefault();
            if (CatId != "-1")
            {
                var products = _unitOfWork.SubCategory.GetAllSubCategories().Where(x => x.CategoryRef == Convert.ToInt32(CatId));
                AdminPanelViewModel model = new AdminPanelViewModel()
                {
                    SubCategories = products,
                };
                return Json(model);
            }
            else
            {
                var products = _unitOfWork.SubCategory.GetAllSubCategories();
                AdminPanelViewModel model = new AdminPanelViewModel()
                {
                    SubCategories = products,
                };
                return Json(model);
            }

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