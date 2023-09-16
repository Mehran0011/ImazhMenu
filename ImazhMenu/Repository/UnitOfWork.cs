using ImazhMenu.Data;
using ImazhMenu.Models;
using ImazhMenu.Repository.IRepository;

namespace ImazhMenu.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
            SubCategory = new SubCategoryRepository(_db);
            Gallery = new GalleryRepository(_db);

        }
        public ICategoryRepository Category { get; private set; }
        public ISubCategoryRepository SubCategory { get; private set; }
        public IGalleryRepository Gallery { get; private set; }


        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
