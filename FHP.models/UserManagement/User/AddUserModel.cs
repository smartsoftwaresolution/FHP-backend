using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.models.UserManagement.User
{
    public class AddUserModel
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string GovernmentId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ContactName { get; set; }
        public string CompanyName { get; set; }
        public string MobileNumber { get; set; }
        public bool? IsVerifyByAdmin { get; set; }

    }
}
