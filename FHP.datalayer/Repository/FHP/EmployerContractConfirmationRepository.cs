
using FHP.dtos.FHP;
using FHP.entity.FHP;
using FHP.infrastructure.Repository.FHP;
using FHP.utilities;
using Microsoft.EntityFrameworkCore;

namespace FHP.datalayer.Repository.FHP
{
    public class EmployerContractConfirmationRepository : IEmployerContractConfirmationRepository
    {
        private readonly DataContext _dataContext;

        public EmployerContractConfirmationRepository(DataContext dataContext)
        {
            _dataContext 
                = dataContext;  
        }

        public async Task AddAsync(EmployerContractConfirmation entity)
        {
            await _dataContext.EmployerContractConfirmations.AddAsync(entity);
            await _dataContext.SaveChangesAsync();
        }

        public void Edit(EmployerContractConfirmation entity)
        {
           _dataContext.EmployerContractConfirmations.Update(entity);
            _dataContext.SaveChanges();
        }


        public async Task<EmployerContractConfirmation> GetAsync(int id)
        {
            return await _dataContext.EmployerContractConfirmations.Where(s => s.Id == id).FirstOrDefaultAsync();
        }

        public async Task<(List<EmployerContractConfirmationDetailDto> employerContract , int totalCount)> GetAllAsync(int page,int pageSize,string? search)
        {
            var query = from s in _dataContext.EmployerContractConfirmations
                        where s.Status != Constants.RecordStatus.Deleted
                        select new { employerContract = s };

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(s => s.employerContract.EmployerId.ToString().Contains(search) ||
                                         s.employerContract.EmployerId.ToString().Contains(search) ||
                                         s.employerContract.JobId.ToString().Contains(search));  
            }


            var totalCount =  await query.CountAsync();

            query = query.OrderByDescending(s => s.employerContract.Id);


            if (page > 0 && pageSize > 0)
            {
                query = query.Skip((page - 1) * pageSize).Take(pageSize);
            }


            var data = await query.Select(s => new EmployerContractConfirmationDetailDto
            {
                Id = s.employerContract.Id,
                EmployeeId = s.employerContract.EmployeeId,
                JobId = s.employerContract.JobId,
                EmployerId = s.employerContract.EmployerId,
                IsSelected = s.employerContract.IsSelected,
                Status = s.employerContract.Status,
                CreatedOn = s.employerContract.CreatedOn,
            }).AsNoTracking().ToListAsync();


            return (data, totalCount);
        }


        public async Task<EmployerContractConfirmationDetailDto> GetByIdAsync(int id)
        {
            return await (from s in _dataContext.EmployerContractConfirmations
                          where s.Status != utilities.Constants.RecordStatus.Deleted &&
                          s.Id  == id

                          select new EmployerContractConfirmationDetailDto
                          {
                              Id = s.Id,
                              EmployeeId = s.EmployeeId,
                              JobId = s.JobId,
                              EmployerId = s.EmployerId,
                              IsSelected = s.IsSelected,
                              Status = s.Status,
                              CreatedOn = s.CreatedOn,
                          }).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var data = await _dataContext.EmployerContractConfirmations.Where(s => s.Id == id).FirstOrDefaultAsync();
            data.Status=utilities.Constants.RecordStatus.Deleted;
            _dataContext.Update(data);
            await _dataContext.SaveChangesAsync();
        }
    }
}
