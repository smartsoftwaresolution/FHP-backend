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

        public async Task<(List<StateDetailDto> state ,int totalCount)> GetAllAsync(int page,int pageSize,string search)
        {
            var query = from s in _dataContext.States
                        where s.Status != Constants.RecordStatus.Deleted
                        select new { state = s };


            var totalCount = await _dataContext.States.CountAsync(s => s.Status != Constants.RecordStatus.Deleted); 

            if (!string.IsNullOrEmpty(search))
            {
                query=query.Where(s=>s.state.StateName.Contains(search));
            }
            
            if(page > 0 && pageSize > 0)
            {
                query = query.Skip((page - 1) * pageSize).Take(pageSize); 
            }

            query = query.OrderByDescending(s => s.state.Id);

          var data =  await query.Select (s=> new StateDetailDto
                                                    {
                                                       Id = s.state.Id,
                                                       StateName = s.state.StateName,
                                                       CountryId = s.state.CountryId,
                                                       CountryName = s.state.Country.CountryName,
                                                       Status = s.state.Status,
                                                       CreatedOn = s.state.CreatedOn,
                                                       UpdatedOn = s.state.UpdatedOn,
                                                    }) 
                                                    .AsNoTracking()
                                                    .ToListAsync();

            return (data,totalCount);

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
                       CountryName = s.Country.CountryName,
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


        public async Task<List<StateDetailDto>> GetByCountryIdAsync(int countryId)
        {
            return await (from s in _dataContext.States
                          where s.Status != utilities.Constants.RecordStatus.Deleted &&
                                s.CountryId == countryId
                          select new StateDetailDto
                          {
                              Id = s.Id,
                              StateName = s.StateName,
                              CountryId = s.CountryId,
                              CountryName = s.Country.CountryName,
                              Status = s.Status,
                              CreatedOn = s.CreatedOn,
                              UpdatedOn = s.UpdatedOn,
                          }).AsNoTracking().ToListAsync();
        }
    }
}
