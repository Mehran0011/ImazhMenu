using Microsoft.AspNetCore.Identity;

namespace ImazhMenu.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string PhoneNumber { get; set; }
    }
}
