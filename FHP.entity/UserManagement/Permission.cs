using FHP.utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace FHP.entity.UserManagement
{
    public class Permission
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string Permissions { get; set; }
        public string PermissionDescription { get; set; }
        public string PermissionCode { get; set; }
        public string ScreenCode { get; set; }
        public string ScreenUrl { get; set; }
        public int ScreenId { get; set; }
        public Screen Screen { get; set; }
        public Constants.RecordStatus Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set;}
        public int CreatedBy { get; set; }
    }
}
