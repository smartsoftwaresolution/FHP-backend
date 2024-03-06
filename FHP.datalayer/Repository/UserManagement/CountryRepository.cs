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
    public class CountryRepository:ICountryRepository
    {
        private readonly DataContext _dataContext;

        public CountryRepository(DataContext dataContext)
        {
            _dataContext=dataContext;
        }

        public async Task AddAsync(Country entity)
        {
            await _dataContext.Countries.AddAsync(entity);
            await _dataContext.SaveChangesAsync();
        }

        public void Edit(Country entity)
        {
            _dataContext.Countries.Update(entity);
            _dataContext.SaveChanges();
        }



        public async Task<Country> GetAsync(int id)
        {
           return await _dataContext.Countries.Where(s => s.Id == id).FirstOrDefaultAsync();
        }

        public async Task<(List<CountryDetailDto> country,int totalCount)> GetAllAsync(int page,int pageSize,string? search)
        {
            var query = from s in _dataContext.Countries
                        where
                        s.Status != Constants.RecordStatus.Deleted
                        select new { country = s };


            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(s => s.country.CountryName.Contains(search));
            }

            var totalCount = await query.CountAsync();


            if (page > 0 && pageSize > 0)
            {
                query =query.Skip((page -1) * pageSize).Take(pageSize);
            }

            query = query.OrderByDescending(s => s.country.Id);

            var data = await query.Select(s => new CountryDetailDto
                                              {
                                                 Id = s.country.Id,
                                                 CountryName = s.country.CountryName,
                                                 Status = s.country.Status,
                                                 CreatedOn = s.country.CreatedOn,
                                                 UpdatedOn = s.country.UpdatedOn,
                                              })
                                             .AsNoTracking()
                                             .ToListAsync();

            return(data, totalCount);
        }

        public async Task<CountryDetailDto> GetByIdAsync(int id)
        {
            return await (from s in _dataContext.Countries
                          where
                  s.Status != utilities.Constants.RecordStatus.Deleted && 
                  s.Id==id 
                          select new CountryDetailDto
                          {
                              Id = s.Id,
                              CountryName = s.CountryName,
                              Status = s.Status,
                              CreatedOn = s.CreatedOn,
                              UpdatedOn = s.UpdatedOn,
                          }).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task DeleteAsync(int id)
        {
             var data =await _dataContext.Countries.Where(s => s.Id == id).FirstOrDefaultAsync();
            data.Status = Constants.RecordStatus.Deleted;
            _dataContext.Update(data);
            await _dataContext.SaveChangesAsync();
        }
    }
}
