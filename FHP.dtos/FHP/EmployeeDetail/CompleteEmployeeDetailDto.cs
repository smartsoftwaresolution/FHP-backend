using FHP.dtos.FHP.EmployeeEducationalDetail;
using FHP.dtos.FHP.EmployeeProfessionalDetail;
using FHP.utilities;

namespace FHP.dtos.FHP.EmployeeDetail
{
    public class CompleteEmployeeDetailDto
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? GovernmentId { get; set; }
        public string RoleName { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public bool? IsVerify { get; set; }
        public Constants.RecordStatus Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string ProfileImg { get; set; }
        public string MobileNumber { get; set; }
        public bool? IsVerifyByAdmin { get; set; }
        public EmployeeDetailDto EmployeeDetail { get; set; }
        public List<EmployeeEducationalDetailDto> Education { get; set; }
        public List<EmployeeProfessionalDetailDto> Professional { get; set; }
    }
}
