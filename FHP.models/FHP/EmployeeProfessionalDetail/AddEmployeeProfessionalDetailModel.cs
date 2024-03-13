using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.models.FHP.EmployeeProfessionalDetail
{
    public class AddEmployeeProfessionalDetailModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string JobDescription { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public DateTime Duration { get; set; }
        public string CompanyName { get; set; }
        public string CompanyLocation { get; set; }
        public string Designation { get; set; }
        public string EmploymentStatus { get; set; }
        public int YearsOfExperience { get; set; }
    }
}
