using Microsoft.EntityFrameworkCore;
using FHP.infrastructure.Repository.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FHP.entity;
using FHP.entity.UserManagement;
using FHP.dtos.UserManagement;
using System.Reflection.Metadata;
using FHP.utilities;

namespace FHP.datalayer.Repository.UserManagement
{
    public class CompanyRepository:ICompanyRepository
    {
        private readonly DataContext _dataContext;

        public CompanyRepository(DataContext dataContext)
        {
            _dataContext= dataContext;
        }

        public async Task  AddAsync(Company entity)
        {
            await _dataContext.Companies.AddAsync(entity);
            await _dataContext.SaveChangesAsync();
           
        }


        public void Edit(Company entity)
        {
            _dataContext.Companies.Update(entity);
            _dataContext.SaveChanges();
        }

        public async Task<List<CompanyDetailDto>> GetAllAsync(int id)
        {
           return await (from s in _dataContext.Companies
                       where s.Status != utilities.Constants.RecordStatus.Deleted && (s.Id == id || s.Id == 0)
                         select new CompanyDetailDto
                       {
                           Id=s.Id,
                           UserId=s.UserId,
                           Name=s.Name,
                           Description=s.Description,
                           Status=s.Status,
                           CreatedOn=s.CreatedOn,
                           UpdatedOn=s.UpdatedOn,
                       }).ToListAsync();
        }

        public async Task<CompanyDetailDto> GetByIdAsync(int Id)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return await (from s in _dataContext.Companies
                          where s.Status != utilities.Constants.RecordStatus.Deleted && s.Id == Id
                          select new CompanyDetailDto
                          {
                              Id=s.Id,
                              UserId=s.UserId,
                              Name=s.Name,
                              Description=s.Description,
                              Status=s.Status,
                              CreatedOn=s.CreatedOn,
                              UpdatedOn=s.UpdatedOn,
                          }).FirstOrDefaultAsync();
#pragma warning restore CS8603 // Possible null reference return.




        }
        public async Task<Company> GetAsync(int id)
        {
            return await _dataContext.Companies.Where(s => s.Id == id ).FirstOrDefaultAsync();
        }


        public async Task DeleteAsync(int id)
        {
            var data = await _dataContext.Companies.Where(s => s.Id == id).FirstOrDefaultAsync();
            data.Status = Constants.RecordStatus.Deleted;
            _dataContext.Companies.Update(data);
            await _dataContext.SaveChangesAsync();
        }
    }
}
