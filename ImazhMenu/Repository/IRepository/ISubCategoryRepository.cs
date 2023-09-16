using ImazhMenu.Models;
using System.Linq.Expressions;

namespace ImazhMenu.Repository.IRepository
{
    public interface ISubCategoryRepository
    {
        IQueryable<Subcategory> GetAllSubCategories();
        void AddSubCategory(Subcategory subcategory);
        void UpdateSubCategory(Subcategory subcategory);
        void DeleteSubCategory(Subcategory subcategory);

    }
}
