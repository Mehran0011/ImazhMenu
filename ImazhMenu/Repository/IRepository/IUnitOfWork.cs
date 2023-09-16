namespace ImazhMenu.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category { get; }
        ISubCategoryRepository SubCategory { get; }
        IGalleryRepository Gallery { get; }

        void Save();


    }
}
