using ImazhMenu.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImazhMenu.ViewModels.Account
{
    public class AdminPanelViewModel
    {
        [Required(ErrorMessage ="لطفا نام محصول را وارد کنید")]
        public string SubCactegoryName { get; set; }
        [Required(ErrorMessage = "لطفا توضیحات محصول را وارد کنید")]
        public string Description { get; set; }
        [Required(ErrorMessage = "لطفا تصویر محصول را وارد کنید")]
        public string SubCatImgUrl { get; set; }
        [Required(ErrorMessage = "لطفا قیمت محصول را وارد کنید")]
        public int Price { get; set; }
        public int CategoryRef { get; set; }
        public IEnumerable<Category> Categories{ get; set; }
        public IEnumerable<Subcategory> SubCategories { get; set; }
        public IEnumerable<Gallery> Gallery { get; set; }



    }
}
