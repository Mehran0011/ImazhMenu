using ImazhMenu.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using ImazhMenu.Data;
using ImazhMenu.Models;
using System.Linq.Expressions;

namespace ImazhMenu.Repository
{
	public class CategoryRepository : ICategoryRepository
	{
		private readonly ApplicationDbContext _db;

		public CategoryRepository(ApplicationDbContext db)
		{
			_db = db;
		}

		public void AddCategory(Category category)
		{
			_db.Add(category);
		}

		public void DeleteCategory(Category category)
		{
			_db.Remove(category);
		}

		public IEnumerable<Category> GetAllCategories()
		{
			return _db.Categories.ToList();
		}

        public void Save()
		{
			_db.SaveChanges();
		}

		public void UpdateCategory(Category category)
		{
			_db.Update(category);
		}
	}
}
