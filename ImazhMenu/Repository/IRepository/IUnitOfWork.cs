namespace ImazhMenu.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category { get; }
        ISubCategoryRepository SubCategory { get; }
        IGalleryRepository Gallery { get; }
        ICustomerClubRepository CustomerClub { get; }

        void Save();


    }
}
