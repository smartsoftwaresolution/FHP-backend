using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.models.UserManagement
{
    public class AddUserModel
    {
        public int Id { get; set;}
        public string RoleName { get; set;}
        
        public string GovernmentId { get; set; }
        public string FullName { get; set;}
        public string Address { get; set; }
        public string Email { get; set;}
        public string Password { get; set;}
        public string MobileNumber { get; set;}
    }
}
