using FHP.entity.UserManagement;
using FHP.utilities;

namespace FHP.dtos.UserManagement.Permission
{
    public class PermissionDetailDto
    {
        public int Id { get; set; }
        public string Permissions { get; set; }
        public string PermissionDescription { get; set; }
        public string PermissionCode { get; set; }
        public string ScreenCode { get; set; }
        public string ScreenUrl { get; set; }
        public int ScreenId { get; set; }
        public Constants.RecordStatus Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int CreatedBy { get; set; }
    }
}
