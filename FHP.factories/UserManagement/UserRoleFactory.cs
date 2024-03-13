using FHP.entity.UserManagement;
using FHP.models.UserManagement.UserRole;
using FHP.utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.factories.UserManagement
{
    public class UserRoleFactory
    {
        public static UserRole Create(AddUserRoleModel model)
        {
            var data = new UserRole
            {
                Id = model.Id,
                RoleName = model.RoleName,
                Status = utilities.Constants.RecordStatus.Active,
                CreatedOn = Utility.GetDateTime(),
            };
            return data;
        }
        public static void Update(UserRole entity,AddUserRoleModel model)
        {
           
            entity.RoleName = model.RoleName;
            entity.UpdatedOn= Utility.GetDateTime();
        }
    }
}
