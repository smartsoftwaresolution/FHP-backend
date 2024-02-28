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
    public class ContractFactory
    {
        public static Contract Create(AddContractModel model)
        {
            var data = new Contract
            {
                EmployeeId= model.EmployeeId,
                JobId= model.JobId,
                EmployerId= model.EmployerId,   
                Duration=Utility.GetDateTime(),
                Description=model.Description,
                EmployeeSignature=model.EmployeeSignature,
                EmployerSignature=model.EmployerSignature,
                StartContract=Utility.GetDateTime(),
                RequestToChangeContract=model.RequestToChangeContract,  
                IsRequestToChangeAccepted=model.IsRequestToChangeAccepted,
                IsSignedByEmployee=model.IsSignedByEmployee,
                IsSignedByEmployer=model.IsSignedByEmployer,
                CreatedOn=Utility.GetDateTime(),
                Status=Constants.RecordStatus.Active,
            };
            return data;
        }

        public static void Update(Contract entity,AddContractModel model)
        {
            entity.EmployeeId = model.EmployeeId;   
            entity.JobId = model.JobId;
            entity.EmployerId= model.EmployerId;
            entity.Duration=Utility.GetDateTime();  
            entity.Description=model.Description;
            entity.EmployeeSignature=model.EmployeeSignature;
            entity.EmployerSignature=model.EmployerSignature;   
            entity.StartContract=Utility.GetDateTime();
            entity.RequestToChangeContract = model.RequestToChangeContract;
            entity.IsRequestToChangeAccepted = model.IsRequestToChangeAccepted;
            entity.IsSignedByEmployee=model.IsSignedByEmployee;
            entity.IsSignedByEmployer = model.IsSignedByEmployer;
            entity.UpdatedOn=Utility.GetDateTime(); 
        }
    }
}
