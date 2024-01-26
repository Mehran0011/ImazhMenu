using ImazhMenu.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using ImazhMenu.Data;
using ImazhMenu.Models;
using System.Linq.Expressions;

namespace ImazhMenu.Repository
{
    public class CustomerClubRepository : ICustomerClubRepository
    {
        private readonly ApplicationDbContext _db;

        public CustomerClubRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public void AddCustomerClubInfo(CustomerClub customerClub)
        {
            _db.Add(customerClub);
        }

        public void DeleteCustomerClubInfo(CustomerClub customerClub)
        {
            _db.Remove(customerClub);
        }

        public IQueryable<CustomerClub> GetAllCustomerClubInfos()
        {
            return _db.CustomerClubs.AsQueryable();
        }
    }
}
