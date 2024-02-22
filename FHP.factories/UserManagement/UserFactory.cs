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
                Status = utilities.Constants.RecordStatus.Active,
                CreatedOn = Utility.GetDateTime()
            };

            return data;
        }
        
        public static void Update(User entity,AddUserModel model)
        {
            
            entity.GovernmentId = model.GovernmentId;
            entity.FirstName = model.FirstName;
            entity.LastName = model.LastName;
            entity.Email = model.Email;
            entity.Password = Utility.Encrypt(model.Password);
            entity.UpdatedOn= Utility.GetDateTime();
        }
    }
}
