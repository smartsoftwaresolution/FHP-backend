using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.models.UserManagement
{
    public class AddEmailSettingModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string AppPassword { get; set; }
        public string IMapHost { get; set; }
        public string IMapPort { get; set; }
        public string SmtpHost { get; set; }
        public string SmtpPort { get; set; }
    }
}
