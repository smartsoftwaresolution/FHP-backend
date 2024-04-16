using FHP.entity.FHP;
using FHP.models.FHP.Contract;
using FHP.models.FHP.GlobalNotification;
using FHP.utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.factories.FHP
{
    public class GlobalNotificationFactory
    {
        public static GlobalNotification Create(AddGlobalNotificationModel model)
        {
            var data = new GlobalNotification
            {
                UserId = model.UserId,
                Name = model.Name,
                Description = model.Description,
                CreatedOn = Utility.GetDateTime(),
                Status = Constants.RecordStatus.Active,
            };
            return data;
        }

        public static void Update(GlobalNotification entity, AddGlobalNotificationModel model)
        {
            entity.UserId = model.UserId;
            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.UpdatedOn = Utility.GetDateTime();
        }

    }
}
