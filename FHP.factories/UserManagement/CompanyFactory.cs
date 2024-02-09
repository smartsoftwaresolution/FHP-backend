using FHP.entity.UserManagement;
using FHP.models.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FHP.utilities;
namespace FHP.factories.UserManagement
{
    public class CompanyFactory
    {
        public static Company Create(AddCompanyModel model)
        {
            var data = new Company
            {
                Id = model.Id,
                UserId = model.UserId,
                Name = model.Name,
                Description = model.Description,
                Status = utilities.Constants.RecordStatus.Active,
                CreatedOn=Utility.GetDateTime(),
            };
            return data;
        }
        public static void Update(Company entity,AddCompanyModel model)
        {
           
            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.UpdatedOn=Utility.GetDateTime();
            entity.UserId = model.UserId;
        }
    }
}
