using FHP.entity.FHP;
using FHP.models.FHP;
using FHP.utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.factories.FHP
{
    public class EmployeeAvailabilityFactory
    {
        public static EmployeeAvailability Create(AddEmployeeAvailabilityModel model)
        {
            var data = new EmployeeAvailability
            {
                UserId = model.UserId,
                EmployeeId = model.EmployeeId,
                JobId = model.JobId,
                IsAvailable = model.IsAvailable,
                CreatedOn = Utility.GetDateTime(),
                Status = Constants.RecordStatus.Active
            };

            return data;
        }

        public static void Update(EmployeeAvailability entity,AddEmployeeAvailabilityModel model)
        {
            entity.UserId = model.UserId;   
            entity.EmployeeId = model.EmployeeId;
            entity.JobId = model.JobId;
            entity.IsAvailable = model.IsAvailable;
        }
    }
}
