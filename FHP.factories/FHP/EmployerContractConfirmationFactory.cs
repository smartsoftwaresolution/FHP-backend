using FHP.entity.FHP;
using FHP.models.FHP.EmployerContractConfirmation;
using FHP.utilities;

namespace FHP.factories.FHP
{
    public  class EmployerContractConfirmationFactory
    {
        public static EmployerContractConfirmation Create(AddEmployerContractConfirmationModel model)
        {
            var data = new EmployerContractConfirmation
            {
                EmployeeId = model.EmployeeId,
                JobId = model.JobId,
                EmployerId = model.EmployerId,
                IsSelected = model.IsSelected,
                CreatedOn=Utility.GetDateTime(),
                Status=Constants.RecordStatus.Active,

            };
            return data;
        }

        public static void Update(EmployerContractConfirmation entity, AddEmployerContractConfirmationModel model)
        {
            entity.EmployeeId = model.EmployeeId;
            entity.JobId = model.JobId;
            entity.EmployerId = model.EmployerId;   
            entity.IsSelected = model.IsSelected;
        }
    }
}
