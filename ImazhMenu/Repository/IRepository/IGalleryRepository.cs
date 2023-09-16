using ImazhMenu.Models;
using System.Linq.Expressions;

namespace ImazhMenu.Repository.IRepository
{
    public interface IGalleryRepository
    {
        IQueryable<Gallery> GetAllGalleryPictures();
        void AddGalleryPicture(Gallery category);
        void UpdateGalleryPicture(Gallery category);
        void DeleteGalleryPicture(Gallery category);

    }
}
