using FHP.entity.FHP;
using FHP.models.FHP;
using FHP.utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.factories.FHP
{
    public class EmployeeProfessionalDetailFactory
    {
        public static EmployeeProfessionalDetail Create(AddEmployeeProfessionalDetailModel model)
        {
            var data = new EmployeeProfessionalDetail
            {
                UserId = model.UserId,
                JobDescription = model.JobDescription,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                CompanyName = model.CompanyName,
                CompanyLocation = model.CompanyLocation,
                Designation = model.Designation,
                EmploymentStatus = model.EmploymentStatus,
                YearsOfExperience = model.YearsOfExperience,
                Status = Constants.RecordStatus.Active,
                CreatedOn = Utility.GetDateTime(),
            };

            return data;
        }

        public static void Update(EmployeeProfessionalDetail entity,AddEmployeeProfessionalDetailModel model)
        {
            entity.UserId = model.UserId;   
            entity.JobDescription= model.JobDescription;
            entity.StartDate= model.StartDate;
            entity.EndDate= model.EndDate;
            entity.CompanyName= model.CompanyName;
            entity.CompanyLocation= model.CompanyLocation;
            entity.Designation= model.Designation;
            entity.EmploymentStatus= model.EmploymentStatus;    
            entity.YearsOfExperience= model.YearsOfExperience;
            entity.UpdatedOn = Utility.GetDateTime();
        }
    }
}
