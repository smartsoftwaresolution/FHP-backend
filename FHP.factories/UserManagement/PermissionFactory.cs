﻿using FHP.entity.UserManagement;
using FHP.models.UserManagement.Permission;
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
        public static Permission Create(AddPermissionModel model)
        {
            var data = new Permission
            {
              
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

        public static void Update(Permission entity, AddPermissionModel model)
        {
           
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
