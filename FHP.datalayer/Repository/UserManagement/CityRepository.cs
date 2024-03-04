

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


        public async Task<(List<CityDetailDto>city,int totalCount)> GetAllAsync(int page,int pageSize,string? search)
         {
            var query = from s in _dataContext.Cities
                        where s.Status != utilities.Constants.RecordStatus.Deleted
                        select new { city = s };


            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(s => s.city.CityName.Contains(search));
            }

            var totalCount = await _dataContext.Cities.CountAsync(s => s.Status != utilities.Constants.RecordStatus.Deleted);

            if (page > 0 && pageSize > 0)
            {
                query = query.Skip((page - 1) * pageSize).Take(pageSize);
            }
                
            query = query.OrderByDescending(s => s.city.Id);

           var data =  await query.Select( s => new CityDetailDto
                                               {
                                                   Id = s.city.Id,
                                                   CityName = s.city.CityName,
                                                   CountryId = s.city.CountryId,
                                                   CountryName = s.city.State.StateName,
                                                   StateId = s.city.StateId,
                                                   StateName = s.city.State.StateName,
                                                   Status = s.city.Status,
                                                   CreatedOn = s.city.CreatedOn,
                                                   UpdatedOn = s.city.UpdatedOn
                                               })
                                                .AsNoTracking()
                                                .ToListAsync();
            return (data, totalCount);
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
                              CountryName = s.Country.CountryName,
                              StateId = s.StateId,
                              StateName = s.State.StateName,
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


        public async Task<List<CityDetailDto> >GetByStateIdAsync(int stateId)
        {
            return await (from s in _dataContext.Cities
                          where s.Status != utilities.Constants.RecordStatus.Deleted &&
                         (s.StateId == stateId)
                          select new CityDetailDto
                          {
                              Id = s.Id,
                              CityName = s.CityName,
                              CountryId = s.CountryId,
                              CountryName = s.Country.CountryName,
                              StateId = s.StateId,
                              StateName = s.State.StateName,
                              Status = s.Status,
                              CreatedOn = s.CreatedOn,
                              UpdatedOn = s.UpdatedOn
                          }).AsNoTracking().ToListAsync();
        }
    }
}
