
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
            _dataContext= dataContext;  
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

        public async Task<List<EmployerContractConfirmationDetailDto>> GetAllAsync()
        {
            return await (from s in _dataContext.EmployerContractConfirmations
                          where s.Status != Constants.RecordStatus.Deleted
                          select new EmployerContractConfirmationDetailDto
                          {
                              Id = s.Id,
                              EmployeeId = s.EmployeeId,
                              JobId = s.JobId,
                              EmployerId = s.EmployerId,
                              IsSelected = s.IsSelected,
                              Status = s.Status,
                              CreatedOn = s.CreatedOn,
                          }).AsNoTracking().ToListAsync();
        }


        public async Task<EmployerContractConfirmationDetailDto> GetByIdAsync(int id)
        {
            return await (from s in _dataContext.EmployerContractConfirmations
                          where s.Status != utilities.Constants.RecordStatus.Deleted &&
                          s.Id  == id
                          select new EmployerContractConfirmationDetailDto
                          {
                              Id=s.Id,
                              EmployeeId=s.EmployeeId,
                              JobId=s.JobId,
                              EmployerId=s.EmployerId,
                              IsSelected=s.IsSelected,
                              Status=s.Status,
                              CreatedOn=s.CreatedOn,
                          }).FirstOrDefaultAsync();
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
