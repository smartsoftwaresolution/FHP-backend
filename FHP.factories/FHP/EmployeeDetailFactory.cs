using FHP.entity.FHP;
using FHP.models.FHP.EmployeeDetail;
using FHP.utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.factories.FHP
{
    public class EmployeeDetailFactory
    {
        public static EmployeeDetail Create(AddEmployeeDetailModel model,string resumeUrl)
        {
            var data = new EmployeeDetail
            {
                UserId = model.UserId,
                MaritalStatus = model.MaritalStatus,
                Gender = model.Gender,
                DOB = model.DOB,
                CountryId = model.CountryId,
                StateId = model.StateId,
                CityId = model.CityId,
                ResumeURL = resumeUrl ?? "",
                ProfileImgURL =  "",
                IsAvailable = model.IsAvailable,
                Hobby = model.Hobby,
                PermanentAddress = model.PermanentAddress,
                AlternateAddress = model.AlternateAddress,
               // Mobile=model.Mobile,
                Phone = model.Phone,
                AlternatePhone = model.AlternatePhone,  
                AlternateEmail = model.AlternateEmail,  
                EmergencyContactName = model.EmergencyContactName,  
                EmergencyContactNumber = model.EmergencyContactNumber,
                Status=Constants.RecordStatus.Active,
                CreatedOn=Utility.GetDateTime(),
            };
            return data;
        }

        public static void Update(EmployeeDetail entity, AddEmployeeDetailModel model,string resumeUrl)
        {
            entity.UserId = model.UserId;
            entity.MaritalStatus= model.MaritalStatus;
            entity.Gender= model.Gender;
            entity.DOB= model.DOB;
            entity.CountryId= model.CountryId;
            entity.StateId= model.StateId;
            entity.CityId= model.CityId;
            entity.ResumeURL= string.IsNullOrEmpty(resumeUrl) ? entity.ResumeURL : resumeUrl;
            entity.ProfileImgURL= entity.ProfileImgURL;
            entity.IsAvailable= model.IsAvailable;
            entity.Hobby= model.Hobby;
            entity.PermanentAddress= model.PermanentAddress;
            entity.AlternateAddress= model.AlternateAddress;
            entity.Phone= model.Phone;
            entity.AlternatePhone= model.AlternatePhone;
            entity.AlternateEmail= model.AlternateEmail;
            entity.EmergencyContactName= model.EmergencyContactName;
            entity.EmergencyContactNumber= model.EmergencyContactNumber;
            entity.UpdatedOn=Utility.GetDateTime();
        }
    }
}
