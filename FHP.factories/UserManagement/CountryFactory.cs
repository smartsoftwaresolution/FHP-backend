using FHP.entity.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FHP.utilities;
using FHP.models.UserManagement.Country;

namespace FHP.factories.UserManagement
{
    public class CountryFactory
    {
        public static Country Create(AddCountryModel model)
        {
            var data = new Country
            {
                CountryName = model.CountryName,
                Status = Constants.RecordStatus.Active,
                CreatedOn=Utility.GetDateTime()
            };

            return data;
        }

        public static void Update(Country entity,AddCountryModel model)
        {
            entity.CountryName = model.CountryName;
            entity.UpdatedOn=Utility.GetDateTime();
        }
    }
}
