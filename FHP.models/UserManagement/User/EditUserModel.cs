using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.models.UserManagement.User
{
    public class EditUserModel
    {
        public int Id { get; set; }
        public string GovernmentId { get; set; }
        public string Email { get; set; }
        public string ContactName { get; set; }
        public string CompanyName { get; set; }
    }
}
