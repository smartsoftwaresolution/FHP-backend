using FHP.dtos.FHP;
using FHP.entity.FHP;
using FHP.infrastructure.Repository.FHP;
using FHP.utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.datalayer.Repository.FHP
{
    public class ContractRepository : IContractRepository
    {
        private readonly DataContext _dataContext;

        public ContractRepository(DataContext dataContext)
        {
            _dataContext=dataContext;
        }

        public async Task AddAsync(Contract entity)
        {
            await _dataContext.Contracts.AddAsync(entity);  
            await _dataContext.SaveChangesAsync();  
        }

        public void Edit(Contract entity)
        {
            _dataContext.Contracts.Update(entity);
            _dataContext.SaveChanges();
        }

        public async Task<Contract> GetAsync(int id)
        {
            return  await _dataContext.Contracts.Where(s => s.Id == id).FirstOrDefaultAsync();
        }

        public async Task<(List<ContractDetailDto> contract, int totalCount)> GetAllAsync(int page, int pageSize, string? search)
        {
            var query = from s in _dataContext.Contracts
                        where s.Status != Constants.RecordStatus.Deleted
                        select new { contract = s };

            var totalCount = await query.CountAsync(s => s.contract.Status != Constants.RecordStatus.Deleted);
            
            if(!string.IsNullOrEmpty(search))
            {
                query =query.Where(s=>s.contract.EmployeeSignature.Contains(search) ||
                                       s.contract.EmployerSignature.Contains(search) ||
                                       s.contract.JobId.ToString().Contains(search));
            }

            if(page > 0 && pageSize > 0)
            {
                query = query.Skip((page - 1) * pageSize).Take(pageSize);
            }

            query = query.OrderByDescending(s => s.contract.Id);

            var data =await query.Select(s=> new ContractDetailDto
            {
                Id = s.contract.Id,
                EmployeeId = s.contract.EmployeeId,
                JobId = s.contract.JobId,
                EmployerId = s.contract.EmployerId,
                Duration = s.contract.Duration,
                Description = s.contract.Description,
                EmployeeSignature = s.contract.EmployeeSignature,
                EmployerSignature = s.contract.EmployerSignature,
                StartContract = s.contract.StartContract,
                RequestToChangeContract = s.contract.RequestToChangeContract,
                IsRequestToChangeAccepted = s.contract.IsRequestToChangeAccepted,
                IsSignedByEmployee = s.contract.IsSignedByEmployee,
                IsSignedByEmployer = s.contract.IsSignedByEmployer,
                CreatedOn = s.contract.CreatedOn,
                UpdatedOn = s.contract.UpdatedOn,
                Status = s.contract.Status,
            }).AsNoTracking().ToListAsync();

            return (data, totalCount);
        }

        public async Task<ContractDetailDto> GetByIdAsync(int id)
        {
           return await (from s in _dataContext.Contracts
                   where s.Status != utilities.Constants.RecordStatus.Deleted &&
                   s.Id==id 

                   select new ContractDetailDto
                   {
                       Id= s.Id,
                       EmployeeId= s.EmployeeId,    
                       JobId= s.JobId,  
                       EmployerId= s.EmployerId,    
                       Duration= s.Duration,
                       Description= s.Description,
                       EmployeeSignature= s.EmployeeSignature,
                       EmployerSignature= s.EmployerSignature,
                       StartContract=s.StartContract,
                       RequestToChangeContract=s.RequestToChangeContract,   
                       IsRequestToChangeAccepted=s.IsRequestToChangeAccepted,
                       IsSignedByEmployee=s.IsSignedByEmployee,
                       IsSignedByEmployer=s.IsSignedByEmployer,
                       CreatedOn=s.CreatedOn,
                       UpdatedOn=s.UpdatedOn,
                       Status=s.Status,
                   }).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task DeleteAsync(int id)
        {
             var data = await _dataContext.Contracts.Where(s => s.Id == id).FirstOrDefaultAsync();
             data.Status = Constants.RecordStatus.Deleted;
             _dataContext.Update(data);
             await _dataContext.SaveChangesAsync();
        }

        
    }
}
