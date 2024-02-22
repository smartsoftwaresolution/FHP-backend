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
        public static Screen Create(AddScreenModel model)
        {
            var data = new Screen
            {
                ScreenName = model.ScreenName,
                ScreenCode = model.ScreenCode,
                Status = utilities.Constants.RecordStatus.Active,
                CreatedOn = Utility.GetDateTime(),
            };
            return data;
        }
        public static void Update(Screen entity,AddScreenModel model)
        {
            entity.ScreenName = model.ScreenName;
            entity.ScreenCode = model.ScreenCode;
            entity.UpdatedOn= Utility.GetDateTime();
        }
    }
}
