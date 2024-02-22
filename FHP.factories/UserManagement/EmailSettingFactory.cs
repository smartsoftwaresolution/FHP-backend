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
    public class EmailSettingFactory
    {
        public static EmailSetting Create(AddEmailSettingModel model, int compnayId)
        {
            var data = new EmailSetting
            {
                CompanyId = compnayId,
                Email = model.Email,
                Password = model.Password,
                AppPassword = model.AppPassword,
                IMapHost = model.IMapHost,
                IMapPort = model.IMapPort,
                SmtpHost = model.SmtpHost,
                SmtpPort = model.SmtpPort,
                Status = utilities.Constants.RecordStatus.Active,
                CreatedOn = Utility.GetDateTime(),
                
            };

            return data;
        }
        public static void Update(EmailSetting entity, AddEmailSettingModel model, int compnayId)
        {
            entity.CompanyId = compnayId;
            entity.Email=model.Email;
            entity.Password=model.Password;
            entity.AppPassword=model.AppPassword;
            entity.IMapHost=model.IMapHost;
            entity.IMapPort=model.IMapPort;
            entity.SmtpHost=model.SmtpHost;
            entity.SmtpPort=model.SmtpPort;
            entity.UpdatedOn = Utility.GetDateTime();
        }
    }
}
