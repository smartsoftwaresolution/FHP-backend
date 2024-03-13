using FHP.entity.UserManagement;
using FHP.models.UserManagement.UserScreenAccess;
using FHP.utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.factories.UserManagement
{
    public class UserScreenAccessFactory
    {
        public static UserScreenAccess Create(AddUserScreenAccessModel model)
        {
            var data = new UserScreenAccess
            {
                RoleId = model.RoleId,
                ScreenId = model.ScreenId,
                Status = Constants.RecordStatus.Active,
                CreatedOn = Utility.GetDateTime()
            };
            return data;
        }
        public static void Update(UserScreenAccess entity, AddUserScreenAccessModel model)
        {
            entity.RoleId = model.RoleId;   
            entity.ScreenId = model.ScreenId;
        }
    }
}
