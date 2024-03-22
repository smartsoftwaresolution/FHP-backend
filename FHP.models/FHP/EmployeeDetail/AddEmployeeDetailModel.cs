using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.models.FHP.EmployeeDetail
{
    public class AddEmployeeDetailModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public string MaritalStatus { get; set; }
        public string Gender { get; set; }
        public DateTime DOB { get; set; }

        public int CountryId { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        public IFormFile? ResumeURL { get; set; }
        public IFormFile? ProfileImgURL { get; set; }
        public bool IsAvailable { get; set; }

        public string Hobby { get; set; }

        public string PermanentAddress { get; set; }
        public string AlternateAddress { get; set; }
        public string Mobile { get; set; }
        public double Phone { get; set; }
        public double AlternatePhone { get; set; }
        public string AlternateEmail { get; set; }
        public string EmergencyContactName { get; set; }
        public int EmergencyContactNumber { get; set; }
    }
}
