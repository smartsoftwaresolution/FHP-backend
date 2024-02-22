

using FHP.dtos.UserManagement;
using FHP.entity.UserManagement;
using FHP.infrastructure.Manager.UserManagement;
using FHP.infrastructure.Repository.UserManagement;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.datalayer.Repository.UserManagement
{
    public class CityRepository : ICityRepository
    {
        private readonly DataContext _dataContext;

        public CityRepository(DataContext dataContext)
        {
            _dataContext= dataContext;
        }

        public async Task AddAsync(City entity)
        {
            await _dataContext.Cities.AddAsync(entity);
            await _dataContext.SaveChangesAsync();
        }

        public void Edit(City entity)
        {
            _dataContext.Cities.Update(entity);
            _dataContext.SaveChanges();
        }

        public async Task<City> GetAsync(int id)
        {
          return await _dataContext.Cities.Where(s => s.Id == id).FirstOrDefaultAsync();
        }


        public async Task<List<CityDetailDto>> GetAllAsync()
        {
           return await (from s in _dataContext.Cities
                   where s.Status != utilities.Constants.RecordStatus.Deleted
                   select new CityDetailDto
                   {
                       Id=s.Id,
                       CityName=s.CityName,
                       CountryId=s.CountryId,
                       StateId=s.StateId,
                       Status=s.Status,
                       CreatedOn = s.CreatedOn, 
                       UpdatedOn = s.UpdatedOn
                   }).AsNoTracking().ToListAsync();
        }

        public async Task<CityDetailDto> GetByIdAsync(int id)
        {
            return await (from s in _dataContext.Cities
                          where s.Status != utilities.Constants.RecordStatus.Deleted &&
                         (s.Id == id)
                          select new CityDetailDto
                          {
                              Id = s.Id,
                              CityName = s.CityName,
                              CountryId = s.CountryId,
                              StateId = s.StateId,
                              Status = s.Status,
                              CreatedOn = s.CreatedOn,
                              UpdatedOn = s.UpdatedOn
                          }).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var data = await _dataContext.Cities.Where(s => s.Id == id).FirstOrDefaultAsync();
            data.Status=utilities.Constants.RecordStatus.Deleted;
            _dataContext.Update(data);
            await _dataContext.SaveChangesAsync();
        }
    }
}
