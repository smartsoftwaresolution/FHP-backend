using FHP.entity.UserManagement;
using FHP.models.UserManagement.City;
using FHP.utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.factories.UserManagement
{
    public class CityFactory
    {
        public static City Create(AddCityModel model)
        {
            var data = new City
            {
                CityName = model.CityName,
                CountryId = model.CountryId,
                StateId = model.StateId,
                Status = Constants.RecordStatus.Active,
                CreatedOn = Utility.GetDateTime(),
            };
            return data;
        }

        public static void Update(City entity,AddCityModel model)
        {
            entity.CityName = model.CityName;
            entity.CountryId = model.CountryId;
            entity.StateId = model.StateId;
            entity.UpdatedOn = Utility.GetDateTime();   
        }
    }
}
