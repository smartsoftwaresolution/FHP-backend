using FHP.dtos.UserManagement;
using FHP.entity.UserManagement;
using FHP.infrastructure.Repository.UserManagement;
using FHP.utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.datalayer.Repository.UserManagement
{
    public  class StateRepository:IStateRepository
    {
        private readonly DataContext _dataContext;

        public StateRepository(DataContext dataContext)
        {
            _dataContext= dataContext;
        }

        public async Task AddAsync(State entity)
        {
           await _dataContext.States.AddAsync(entity);
           await _dataContext.SaveChangesAsync();  
        }

        public void Edit(State entity)
        {
            _dataContext.States.Update(entity);
            _dataContext.SaveChanges();
        }


        public async Task<State> GetAsync(int id)
        {
           return  await _dataContext.States.Where(s => s.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<StateDetailDto>> GetAllAsync()
        {
          return  await (from s in _dataContext.States
                   where s.Status != utilities.Constants.RecordStatus.Deleted
                   select new StateDetailDto
                   {
                       Id=s.Id,
                       StateName=s.StateName,
                       CountryId=s.CountryId,
                       Status=s.Status,
                       CreatedOn=s.CreatedOn,
                       UpdatedOn=s.UpdatedOn,
                   }).AsNoTracking().ToListAsync();
        }


        public async Task<StateDetailDto> GetByIdAsync(int id)
        {
           return await (from s in _dataContext.States
                   where
                   s.Status != utilities.Constants.RecordStatus.Deleted &&
                   s.Id == id
                   select new StateDetailDto
                   {
                       Id=s.Id,
                       StateName=s.StateName,
                       CountryId=s.CountryId,
                       Status=s.Status,
                       CreatedOn=s.CreatedOn,
                       UpdatedOn=s.UpdatedOn,
                   }).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task DeleteAsync(int id)
        {
           var data= await _dataContext.States.Where(s => s.Id == id).FirstOrDefaultAsync();
            data.Status = Constants.RecordStatus.Deleted;
            _dataContext.Update(data);
            await _dataContext.SaveChangesAsync();
        }
    }
}
