using ImazhMenu.Models;
using ImazhMenu.Repository.IRepository;
using ImazhMenu.ViewModels;
using ImazhMenu.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using NToastNotify;
using NuGet.Protocol.Core.Types;
using System.Security.Claims;

namespace ImazhMenu.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IToastNotification _toastNotification;
        private readonly IWebHostEnvironment _hostEnvironment;


        public AccountController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IUnitOfWork unitOfWork,
            IToastNotification toastNotification,
            IWebHostEnvironment hostEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _toastNotification = toastNotification;
            _hostEnvironment = hostEnvironment;

        }


        public IActionResult Register()
        {
            return View();
        }
        public IActionResult AdminPanel()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }
            return View(model);
        }

        public IActionResult Login()
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (_signInManager.IsSignedIn(User))
                return RedirectToAction("Index", "Home");
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(
                    model.UserName, model.Password, model.RememberMe, true);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                if (result.IsLockedOut)
                {
                    ViewData["ErrorMessage"] = "Your Account is Suspended";
                    return View(model);
                }

                ModelState.AddModelError("", "UserName Or Password is incorect");
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        //=====================================================================================
        [HttpPost]
        public virtual async Task<JsonResult> GetAllCategories()
        {

            var draw = HttpContext.Request.Form["draw"].FirstOrDefault();

            // Skip number of Rows count  
            var start = Request.Form["start"].FirstOrDefault();

            // Paging Length 10,20  
            var length = Request.Form["length"].FirstOrDefault();

            // Sort Column Name  
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();

            // Sort Column Direction (asc, desc)  
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();

            // Search Value from (Search box)  
            var searchValue = Request.Form["search[value]"].FirstOrDefault();

            //Paging Size (10, 20, 50,100)  
            int pageSize = length != null ? Convert.ToInt32(length) : 0;

            int skip = start != null ? Convert.ToInt32(start) : 0;

            int recordsTotal = 0;


            IQueryable<Category> _result = _unitOfWork.Category.GetAllCategories();
            if (_result != null)
            {
                if (!string.IsNullOrEmpty(searchValue) && !string.IsNullOrWhiteSpace(searchValue))
                {
                    // Apply search    
                    _result = _result.Where(p => p.CactegoryName.ToLower().Contains(searchValue.ToLower()));

                }


                var _resultfinal = _result
                        .Select(x => new
                        {
                            id = x.Id,
                            categoryName = x.CactegoryName
                        });

                //Sorting  datatable
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    //_resultfinal = _resultfinal.OrderBy(sortColumn + " " + sortColumnDirection);
                }
                //total number of rows counts   
                recordsTotal = _resultfinal.Count();
                //Paging   
                var data = _resultfinal.OrderByDescending(x => x.categoryName).Skip(skip).Take(pageSize).ToList();

                //Returning Json Data  
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

            }
            else
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = _result });

        }

        public IActionResult CreateNewCategory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateNewCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.AddCategory(category);
                _unitOfWork.Save();
                _toastNotification.AddSuccessToastMessage("دسته بندی با موفقیت افزوده شد");
            }
            else
            {
                _toastNotification.AddWarningToastMessage("در ثبت دسته بندی مشکلی پیش آمده است");
            }
            return View(category);
        }

        public IActionResult UpdateCategory(int id)
        {
            var obj = _unitOfWork.Category.GetAllCategories().Where(u => u.Id == id).FirstOrDefault();
            return View(obj);
        }

        [HttpPost]
        public IActionResult UpdateCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.UpdateCategory(category);
                _unitOfWork.Save();
                _toastNotification.AddSuccessToastMessage("دسته بندی با موفقیت ویرایش شد");
            }
            else
            {
                _toastNotification.AddWarningToastMessage("در ویرایش دسته بندی مشکلی پیش آمده است");
            }
            return RedirectToAction("CreateNewCategory");
        }
        [HttpDelete]
        public IActionResult DeleteCategory(int? id)
        {
            var obj = _unitOfWork.Category.GetAllCategories().Where(u => u.Id == id).FirstOrDefault();
            if (obj == null)
            {
                NotFound();
            }
            else
            {
                _unitOfWork.Category.DeleteCategory(obj);
                _unitOfWork.Save();
                _toastNotification.AddSuccessToastMessage("دسته بندی با موفقیت حذف شد");
            }

            return Json(new { success = true, message = "Deleted Successfully!" });
        }
        //=====================================================================================
        [HttpPost]
        public virtual async Task<JsonResult> GetAllSubCategories()
        {

            var draw = HttpContext.Request.Form["draw"].FirstOrDefault();

            // Skip number of Rows count  
            var start = Request.Form["start"].FirstOrDefault();

            // Paging Length 10,20  
            var length = Request.Form["length"].FirstOrDefault();

            // Sort Column Name  
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();

            // Sort Column Direction (asc, desc)  
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();

            // Search Value from (Search box)  
            var searchValue = Request.Form["search[value]"].FirstOrDefault();

            //Paging Size (10, 20, 50,100)  
            int pageSize = length != null ? Convert.ToInt32(length) : 0;

            int skip = start != null ? Convert.ToInt32(start) : 0;

            int recordsTotal = 0;


            IQueryable<Subcategory> _result = _unitOfWork.SubCategory.GetAllSubCategories();
            if (_result != null)
            {
                if (!string.IsNullOrEmpty(searchValue) && !string.IsNullOrWhiteSpace(searchValue))
                {
                    // Apply search    
                    _result = _result.Where(p => p.SubCactegoryName.ToLower().Contains(searchValue.ToLower()));

                }


                var _resultfinal = _result
                        .Select(x => new
                        {
                            id = x.Id,
                            subCategoryName = x.SubCactegoryName,
                            price = x.Price,
                            subCatImage = x.SubCatImgUrl,
                            subCatDesc = x.Description
                        });

                //Sorting  datatable
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    //_resultfinal = _resultfinal.OrderBy(sortColumn + " " + sortColumnDirection);
                }
                //total number of rows counts   
                recordsTotal = _resultfinal.Count();
                //Paging   
                var data = _resultfinal.OrderByDescending(x => x.subCategoryName).Skip(skip).Take(pageSize).ToList();

                //Returning Json Data  
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

            }
            else
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = _result });

        }

        public IActionResult CreateNewSubCategory()
        {
            var Categories = _unitOfWork.Category.GetAllCategories();
            AdminPanelViewModel model = new AdminPanelViewModel()
            {
                Categories = Categories
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult CreateNewSubCategory(AdminPanelViewModel model, int categoryId, IFormFile? file)
        {
            var Categories = _unitOfWork.Category.GetAllCategories();
            AdminPanelViewModel catmodel = new AdminPanelViewModel()
            {
                Categories = Categories
            };

            Category Category = _unitOfWork.Category.GetAllCategories().Where(x => x.Id == categoryId).FirstOrDefault();
            model.CategoryRef = categoryId;
            model.Categories = Categories;
            Subcategory _subcat = new Subcategory()
            {
                SubCactegoryName = model.SubCactegoryName,
                CategoryRef = model.CategoryRef,
                Category = Category,
                Description = model.Description,
                Price = model.Price
            };
            if (model.SubCactegoryName != "" && model.CategoryRef != -1 && model.Price != 0)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = file.FileName;
                    var uploads = Path.Combine(wwwRootPath, @"Images\Products");
                    string extension = Path.GetExtension(file.FileName);

                    if (file != null)
                    {
                        var oldImage = Path.Combine(uploads, fileName);
                        if (System.IO.File.Exists(oldImage))
                        {
                            System.IO.File.Delete(oldImage);
                            using (var fileStreams = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                            {
                                file.CopyTo(fileStreams);
                            }
                            _subcat.SubCatImgUrl = @"Images/Products/" + fileName;
                        }
                        else
                        {
                            using (var fileStreams = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                            {
                                file.CopyTo(fileStreams);
                            }
                            _subcat.SubCatImgUrl = @"Images/Products/" + fileName;
                        }
                    }
                    _unitOfWork.SubCategory.AddSubCategory(_subcat);
                    _unitOfWork.Save();
                    _toastNotification.AddSuccessToastMessage("محصول با موفقیت افزوده شد");
                    return View(model);
                }
                else
                {
                    _toastNotification.AddWarningToastMessage("در ثبت محصول مشکلی پیش آمده است");
                    return View(catmodel);
                }
            }
            return View(catmodel);
        }

        public IActionResult UpdateSubCategory(int id)
        {
            var obj = _unitOfWork.SubCategory.GetAllSubCategories().Where(u => u.Id == id).FirstOrDefault();
            return View(obj);
        }

        [HttpPost]
        public IActionResult UpdateSubCategory(Subcategory Subcategory, IFormFile? file)
        {
            if (Subcategory.SubCactegoryName != "" && Subcategory.CategoryRef != -1 && Subcategory.Price != 0)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = file.FileName;
                    var uploads = Path.Combine(wwwRootPath, @"Images\Products");
                    string extension = Path.GetExtension(file.FileName);

                    if (file != null)
                    {
                        var oldImage = Path.Combine(uploads, fileName);
                        if (System.IO.File.Exists(oldImage))
                        {
                            System.IO.File.Delete(oldImage);
                            using (var fileStreams = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                            {
                                file.CopyTo(fileStreams);
                            }
                            Subcategory.SubCatImgUrl = @"Images/Products/" + fileName;
                        }
                        else
                        {
                            using (var fileStreams = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                            {
                                file.CopyTo(fileStreams);
                            }
                            Subcategory.SubCatImgUrl = @"Images/Products/" + fileName;
                        }
                    }
                    _unitOfWork.SubCategory.UpdateSubCategory(Subcategory);
                    _unitOfWork.Save();
                    _toastNotification.AddSuccessToastMessage("محصول با موفقیت ویرایش شد");
                }
                else
                {
                    if (file == null)
                    {
                        _toastNotification.AddWarningToastMessage("لطفا تصویر محصول را انتخاب کنید");

                    }
                    _toastNotification.AddWarningToastMessage("در ویرایش محصول مشکلی پیش آمده است");
                }
            }
            return RedirectToAction("CreateNewSubCategory");

        }
        [HttpDelete]
        public IActionResult DeleteSubCategory(int? id)
        {
            var obj = _unitOfWork.SubCategory.GetAllSubCategories().Where(u => u.Id == id).FirstOrDefault();
            if (obj == null)
            {
                NotFound();
            }
            else
            {
                _unitOfWork.SubCategory.DeleteSubCategory(obj);
                _unitOfWork.Save();
                _toastNotification.AddSuccessToastMessage("محصول با موفقیت حذف شد");
            }

            return Json(new { success = true, message = "Deleted Successfully!" });
        }
        //=====================================================================================
        [HttpPost]
        public virtual async Task<JsonResult> GetAllGalleryPictures()
        {

            var draw = HttpContext.Request.Form["draw"].FirstOrDefault();

            // Skip number of Rows count  
            var start = Request.Form["start"].FirstOrDefault();

            // Paging Length 10,20  
            var length = Request.Form["length"].FirstOrDefault();

            // Sort Column Name  
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();

            // Sort Column Direction (asc, desc)  
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();

            // Search Value from (Search box)  
            var searchValue = Request.Form["search[value]"].FirstOrDefault();

            //Paging Size (10, 20, 50,100)  
            int pageSize = length != null ? Convert.ToInt32(length) : 0;

            int skip = start != null ? Convert.ToInt32(start) : 0;

            int recordsTotal = 0;


            IQueryable<Gallery> _result = _unitOfWork.Gallery.GetAllGalleryPictures();
            if (_result != null)
            {
                if (!string.IsNullOrEmpty(searchValue) && !string.IsNullOrWhiteSpace(searchValue))
                {

                }


                var _resultfinal = _result
                        .Select(x => new
                        {
                            id = x.Id,
                            galleryPicUrl = x.ImgUrl
                        });

                //Sorting  datatable
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    //_resultfinal = _resultfinal.OrderBy(sortColumn + " " + sortColumnDirection);
                }
                //total number of rows counts   
                recordsTotal = _resultfinal.Count();
                //Paging   
                var data = _resultfinal.OrderByDescending(x => x.id).Skip(skip).Take(pageSize).ToList();

                //Returning Json Data  
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

            }
            else
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = _result });

        }

        public IActionResult Gallery()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Gallery(IEnumerable<IFormFile>? file)
        {
            Gallery _gallery = new Gallery();
            string wwwRootPath = _hostEnvironment.WebRootPath;
            if (file != null)
            {
                try
                {
                    foreach (var item in file)
                    {
                        string fileName = item.FileName;
                        var uploads = Path.Combine(wwwRootPath, @"Images\Gallery");
                        string extension = Path.GetExtension(item.FileName);

                        if (file != null)
                        {
                            var oldImage = Path.Combine(uploads, fileName);
                            if (System.IO.File.Exists(oldImage))
                            {
                                System.IO.File.Delete(oldImage);
                                using (var fileStreams = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                                {
                                    item.CopyTo(fileStreams);
                                }
                                _gallery.ImgUrl = @"Images/Gallery/" + fileName;
                            }
                            else
                            {
                                using (var fileStreams = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                                {
                                    item.CopyTo(fileStreams);
                                }
                                _gallery.ImgUrl = @"Images/Gallery/" + fileName;
                            }
                            _unitOfWork.Gallery.AddGalleryPicture(_gallery);
                        }
                        _unitOfWork.Save();
                    }
                }
                catch
                {
                    _toastNotification.AddWarningToastMessage("در ثبت تصاویر گالری مشکلی پیش آمده است");

                }

                _toastNotification.AddSuccessToastMessage("تصاویر گالری با موفقیت افزوده شد");
                return View(_gallery);

            }
            else
            {
                _toastNotification.AddWarningToastMessage("در ثبت تصاویر گالری مشکلی پیش آمده است");
                return View(_gallery);
            }
        }


        public IActionResult UpdateGalleryPicture(int id)
        {
            var obj = _unitOfWork.Gallery.GetAllGalleryPictures().Where(u => u.Id == id).FirstOrDefault();
            return View(obj);
        }

        [HttpPost]
        public IActionResult UpdateGalleryPicture(Gallery _gallery,string oldimageurl, IFormFile? file)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            if (file != null)
            {
                try
                {
                    string fileName = file.FileName;
                    var uploads = Path.Combine(wwwRootPath, @"Images\Gallery");
                    string extension = Path.GetExtension(file.FileName);

                    if (file != null)
                    {
                        var oldImage = Path.Combine(wwwRootPath, oldimageurl.Replace('/','\\'));
                        if (System.IO.File.Exists(oldImage))
                        {
                            System.IO.File.Delete(oldImage);
                            using (var fileStreams = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                            {
                                file.CopyTo(fileStreams);
                            }
                            _gallery.ImgUrl = @"Images/Gallery/" + fileName;
                        }
                        else
                        {
                            using (var fileStreams = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                            {
                                file.CopyTo(fileStreams);
                            }
                            _gallery.ImgUrl = @"Images/Gallery/" + fileName;
                        }
                        _unitOfWork.Gallery.UpdateGalleryPicture(_gallery);
                    }
                    _unitOfWork.Save();

                }
                catch
                {
                    _toastNotification.AddWarningToastMessage("در ثبت تصاویر گالری مشکلی پیش آمده است");
                }
                _toastNotification.AddSuccessToastMessage("تصاویر گالری با موفقیت ویرایش شد");
                return RedirectToAction("Gallery");
            }
            else
            {
                _toastNotification.AddWarningToastMessage("در ویرایش تصویر گالری مشکلی پیش آمده است");
                return RedirectToAction("Gallery");
            }
        }

        [HttpDelete]
        public IActionResult DeleteGalleryPicture(int? id)
        {
            var obj = _unitOfWork.Gallery.GetAllGalleryPictures().Where(u => u.Id == id).FirstOrDefault();
            if (obj == null)
            {
                NotFound();
            }
            else
            {
                _unitOfWork.Gallery.DeleteGalleryPicture(obj);
                _unitOfWork.Save();
                _toastNotification.AddSuccessToastMessage("تصویر گالری با موفقیت حذف شد");
            }

            return Json(new { success = true, message = "Deleted Successfully!" });
        }
    }
}

