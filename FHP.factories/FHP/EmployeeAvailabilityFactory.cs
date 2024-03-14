using FHP.entity.FHP;
using FHP.models.FHP.EmployeeAvailability;
using FHP.utilities;

namespace FHP.factories.FHP
{
    public class EmployeeAvailabilityFactory
    {
        public static EmployeeAvailability Create(AddEmployeeAvailabilityModel model)
        {
            var data = new EmployeeAvailability
            {
                UserId = model.UserId,
              //  EmployeeId = model.EmployeeId,
                JobId = model.JobId,
                IsAvailable = Constants.EmployeeAvailability.Pending,
                CreatedOn = Utility.GetDateTime(),
                Status = Constants.RecordStatus.Active
            };

            return data;
        }

        public static void Update(EmployeeAvailability entity,AddEmployeeAvailabilityModel model)
        {
            entity.UserId = model.UserId;   
          //  entity.EmployeeId = model.EmployeeId;
            entity.JobId = model.JobId;
            entity.IsAvailable = model.IsAvailable;
        }
    }
}
