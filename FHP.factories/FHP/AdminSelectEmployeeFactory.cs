using FHP.entity.FHP;
using FHP.models.FHP.AdminSelectEmployee;
using FHP.utilities;

namespace FHP.factories.FHP
{
    public class AdminSelectEmployeeFactory
    {
        public  static AdminSelectEmployee Create(AddAdminSelectEmployeeModel model)
        {
            var data = new AdminSelectEmployee
            {
                JobId = model.JobId,
             // EmployeeId = model.EmployeeId,
                InProbationCancel = model.InProbationCancel,
                IsSelected = model.IsSelected,
                Status = Constants.ProcessingStatus.InProcess,
                CreatedOn = Utility.GetDateTime()
            };
            return data;
        } 

        public static void Update(AdminSelectEmployee entity,AddAdminSelectEmployeeModel model)
        {
            entity.JobId = model.JobId;
         // entity.EmployeeId = model.EmployeeId;

        }
    }
}
