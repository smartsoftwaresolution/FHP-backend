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
    public class PermissionFactory
    {
        public static Permission Create(AddPermissionModel model,int companyId)
        {
            var data = new Permission
            {
                Id = model.Id,
                CompanyId = companyId,
                Permissions = model.Permissions,
                PermissionDescription = model.PermissionDescription,
                PermissionCode = model.PermissionCode,
                ScreenCode = model.ScreenCode,
                ScreenUrl = model.ScreenUrl,
                ScreenId = model.ScreenId,
                CreatedBy= model.CreatedBy,
                Status = utilities.Constants.RecordStatus.Active,
                CreatedOn = Utility.GetDateTime()
            };
            return data;
        }

        public static void Update(Permission entity, AddPermissionModel model, int companyId)
        {
            entity.CompanyId = companyId;
            entity.Permissions = model.Permissions;
            entity.PermissionDescription = model.PermissionDescription;
            entity.PermissionCode = model.PermissionCode;
            entity.ScreenCode = model.ScreenCode;
            entity.ScreenUrl = model.ScreenUrl;
            entity.ScreenId = model.ScreenId;
            entity.UpdatedOn = Utility.GetDateTime();
        }
    }
}
