using FHP.utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.dtos.FHP.EmployeeDetail
{
    public class EmployeeDetailDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public string MaritalStatus { get; set; }
        public string Gender { get; set; }
        public DateTime? DOB { get; set; }

        public int? CountryId { get; set; }
        public int? StateId { get; set; }
        public int? CityId { get; set; }
        public string CountryName { get; set; }
        public string StateName { get; set; }
        public string CityName { get; set; }
        public string ResumeURL { get; set; }
        public string ProfileImgURL { get; set; }
        public bool? IsAvailable { get; set; }

        public string Hobby { get; set; }

        public string PermanentAddress { get; set; }
        public string AlternateAddress { get; set; }
        public string Mobile { get; set; }
        public double? Phone { get; set; }
        public double? AlternatePhone { get; set; }
        public string AlternateEmail { get; set; }
        public string EmergencyContactName { get; set; }
        public int? EmergencyContactNumber { get; set; }


        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public Constants.RecordStatus Status { get; set; }



    }
}
