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
    public class ScreenFactory
    {
        public static Screen Create(AddScreenModel model, int CompanyId)
        {
            var data = new Screen
            {
                Id = model.Id,
                CompanyId = CompanyId,
                ScreenName = model.ScreenName,
                ScreenCode = model.ScreenCode,
                Status = utilities.Constants.RecordStatus.Active,
                CreatedOn = Utility.GetDateTime(),
            };
            return data;
        }
        public static void Update(Screen entity,AddScreenModel model, int CompanyId)
        {
            entity.CompanyId = CompanyId;
            entity.ScreenName = model.ScreenName;
            entity.ScreenCode = model.ScreenCode;
            entity.UpdatedOn= Utility.GetDateTime();
        }
    }
}
