using FHP.utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.entity.FHP
{
   
        public class EmployeeProfessionalDetail
        {
            public int Id { get; set; }
            public int UserId { get; set; }
            public string JobDescription { get; set; }
            public string StartDate { get; set; }
            public string EndDate { get; set; }
            public string CompanyName { get; set; }
            public string CompanyLocation { get; set; }
            public string Designation { get; set; }
            public string EmploymentStatus { get; set; }
            public string YearsOfExperience { get; set; }
            public DateTime CreatedOn { get; set; }
            public DateTime? UpdatedOn { get; set; }
            public Constants.RecordStatus Status { get; set; }
    }

    
}
