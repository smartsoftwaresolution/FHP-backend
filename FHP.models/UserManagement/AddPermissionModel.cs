using FHP.entity.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.models.UserManagement
{
    public class AddPermissionModel
    {
        public int Id { get; set; }
        public string Permissions { get; set; }
        public string PermissionDescription { get; set; }
        public string PermissionCode { get; set; }
        public string ScreenCode { get; set; }
        public string ScreenUrl { get; set; }
        public int ScreenId { get; set; }
        public int CreatedBy { get; set; }
    }
}
