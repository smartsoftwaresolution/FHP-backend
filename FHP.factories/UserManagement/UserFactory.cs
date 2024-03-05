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
        public static User Create(AddUserModel model)
        {
            var data = new User
            {
                GovernmentId = model.GovernmentId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Password = Utility.Encrypt(model.Password),
                CompanyName = model.CompanyName,
                ContactName = model.ContactName,
                Status = utilities.Constants.RecordStatus.Active,
                CreatedOn = Utility.GetDateTime(),
                MobileNumber = model.MobileNumber,
            };

            return data;
        }
        
        public static void Update(User entity,AddUserModel model)
        {
            entity.FirstName = model.CompanyName ?? "";
            entity.LastName = model.CompanyName ?? "";

            entity.GovernmentId = model.GovernmentId;
            entity.FirstName = model.FirstName ?? "";
            entity.LastName = model.LastName ?? "";
            entity.Email = model.Email;
            entity.CompanyName = model.CompanyName;
            entity.ContactName = model.ContactName;
            entity.Password = entity.Password;
            entity.UpdatedOn= Utility.GetDateTime();
            entity.MobileNumber = model.MobileNumber;
        }
    }
}
