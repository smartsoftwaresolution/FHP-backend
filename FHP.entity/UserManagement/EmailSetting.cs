using FHP.utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace FHP.entity.UserManagement
{
    public class EmailSetting
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string AppPassword { get; set; }
        public string IMapHost { get; set; }
        public string IMapPort { get; set; }
        public string SmtpHost { get; set; }
        public string SmtpPort { get; set; }
        public Constants.RecordStatus Status { get; set;}
        public DateTime CreatedOn { get; set;}
        public DateTime? UpdatedOn { get; set; }
    }
}
