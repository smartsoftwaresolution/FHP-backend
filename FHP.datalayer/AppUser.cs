using FHP.utilities;
using Microsoft.AspNetCore.Identity;

namespace FHP.datalayer
{
    public class AppUser:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? LastLogOn { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Role { get; set; }
        public Constants.RecordStatus Status { get; set; }
    }
}
