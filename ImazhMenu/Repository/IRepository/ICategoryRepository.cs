using ImazhMenu.Models;
using System.Linq.Expressions;

namespace ImazhMenu.Repository.IRepository
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetAllCategories();
        void AddCategory(Category category);
        void UpdateCategory(Category category);
        void DeleteCategory(Category category);
        void Save();

    }
}
