using ImazhMenu.Models;
using System.Linq.Expressions;

namespace ImazhMenu.Repository.IRepository
{
    public interface ICustomerClubRepository
    {
        IQueryable<CustomerClub> GetAllCustomerClubInfos();
        void AddCustomerClubInfo(CustomerClub customerClub);
        void DeleteCustomerClubInfo(CustomerClub customerClub);
    }
}
