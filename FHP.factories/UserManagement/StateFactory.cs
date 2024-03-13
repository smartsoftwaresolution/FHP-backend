using FHP.entity.UserManagement;
using FHP.models.UserManagement.State;
using FHP.utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.factories.UserManagement
{
    public class StateFactory
    {
        public static State Create(AddStateModel model)
        {
            var data = new State
            {
                StateName = model.StateName,
                CountryId = model.CountryId,
                Status = Constants.RecordStatus.Active,
                CreatedOn = Utility.GetDateTime(),
            };
            return data;
        }
        public static void Update(State entity, AddStateModel model)
        {
            entity.StateName= model.StateName;
            entity.CountryId= model.CountryId;
            entity.UpdatedOn = Utility.GetDateTime();   
        }
    }
}
