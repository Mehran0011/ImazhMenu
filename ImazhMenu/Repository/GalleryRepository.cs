using ImazhMenu.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using ImazhMenu.Data;
using ImazhMenu.Models;
using System.Linq.Expressions;

namespace ImazhMenu.Repository
{
	public class GalleryRepository : IGalleryRepository
	{
		private readonly ApplicationDbContext _db;

		public GalleryRepository(ApplicationDbContext db)
		{
			_db = db;
		}

		public void AddGalleryPicture(Gallery gallery)
		{
			_db.Add(gallery);
		}

		public void DeleteGalleryPicture(Gallery gallery)
		{
			_db.Remove(gallery);
		}

		public IQueryable<Gallery> GetAllGalleryPictures()
		{
			return _db.Galleries.AsQueryable();
		}

		public void UpdateGalleryPicture(Gallery gallery)
		{
			_db.Update(gallery);
		}
	}
}
