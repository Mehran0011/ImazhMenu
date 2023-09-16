using ImazhMenu.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using ImazhMenu.Data;
using ImazhMenu.Models;
using System.Linq.Expressions;

namespace ImazhMenu.Repository
{
	public class SubCategoryRepository : ISubCategoryRepository
	{
		private readonly ApplicationDbContext _db;

		public SubCategoryRepository(ApplicationDbContext db)
		{
			_db = db;
		}

		public void AddSubCategory(Subcategory subcategory)
		{
			_db.Add(subcategory);
		}

		public void DeleteSubCategory(Subcategory subcategory)
		{
			_db.Remove(subcategory);
		}

		public IQueryable<Subcategory> GetAllSubCategories()
		{
			return _db.Subcategories.AsQueryable();
		}

		public void UpdateSubCategory(Subcategory subcategory)
		{
			_db.Update(subcategory);
		}
	}
}
