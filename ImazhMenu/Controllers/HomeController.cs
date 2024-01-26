using ImazhMenu.Models;
using ImazhMenu.Repository.IRepository;
using ImazhMenu.ViewModels.Account;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using NuGet.Protocol.Core.Types;
using System.Diagnostics;

namespace ImazhMenu.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IToastNotification _toastNotification;


        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork, IToastNotification toastNotification)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _toastNotification = toastNotification;

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
                //if (model.Description = model.Description ? null) ;
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

        //=====================================================================================
        [HttpPost]
        public IActionResult AddCustomerClubInfo(string name, string phonenumber, string subject, string message)
        {
            CustomerClub customerClub = new CustomerClub();
            customerClub.CustomerName = name;
            customerClub.PhoneNumber = phonenumber;
            customerClub.Subject = subject;
            customerClub.MessageDesc = message;

            if (customerClub != null)
            {
                _unitOfWork.CustomerClub.AddCustomerClubInfo(customerClub);
                _unitOfWork.Save();
                _toastNotification.AddSuccessToastMessage("پیام شما با موفقیت ارسال شد");
                return RedirectToAction("Index");

            }
            else
            {
                _toastNotification.AddWarningToastMessage("در ارسال پیام مشکلی پیش آمده است");
                return RedirectToAction("Index");

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