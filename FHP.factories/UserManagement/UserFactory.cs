using FHP.entity.UserManagement;
using FHP.models.UserManagement;
using FHP.utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.factories.UserManagement
{
    public  class UserFactory
    {
        public static User Create(AddUserModel model, int companyId)
        {
            var data = new User
            {
                Id = model.Id,
                CompanyId = companyId,
                GovernmentId = model.GovernmentId,
                FullName = model.FullName,
                Address = model.Address,
                Email = model.Email,
                Password = Utility.Encrypt(model.Password),
                MobileNumber = model.MobileNumber,
                Status = utilities.Constants.RecordStatus.Active,
                CreatedOn = Utility.GetDateTime()
            };

            return data;
        }
        
        public static void Update(User entity,AddUserModel model,int companyId)
        {
            entity.CompanyId = companyId;
            entity.GovernmentId = model.GovernmentId;
            entity.FullName = model.FullName;
            entity.Address = model.Address;
            entity.Email = model.Email;
            entity.Password = Utility.Encrypt(model.Password);
            entity.MobileNumber = model.MobileNumber;
            entity.UpdatedOn= Utility.GetDateTime();
        }
    }
}
