﻿using FHP.dtos.FHP;
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
    public class EmployeeEducationalDetailRepository : IEmployeeEducationalDetailRepository
    {
        private readonly DataContext _dataContext;

        public EmployeeEducationalDetailRepository(DataContext dataContext)
        {
            _dataContext= dataContext;
        }
        public async Task AddAsync(EmployeeEducationalDetail entity)
        {
            await _dataContext.EmployeeEducationalDetails.AddAsync(entity);
            await _dataContext.SaveChangesAsync();
        }

        public void Edit(EmployeeEducationalDetail entity)
        {
           _dataContext.EmployeeEducationalDetails.Update(entity);
            _dataContext.SaveChanges();
        }

        public async Task<EmployeeEducationalDetail> GetAsync(int id)
        {
           return  await _dataContext.EmployeeEducationalDetails.Where(s => s.Id == id).FirstOrDefaultAsync();
        }


        public async Task<(List<EmployeeEducationalDetailDetailDto> employeeeducationaldetail, int totalCount)> GetAllAsync(int page, int pageSize, string? search)
        {
            var query = from s in _dataContext.EmployeeEducationalDetails
                        join t in _dataContext.User on s.UserId equals t.Id
                        where s.Status != utilities.Constants.RecordStatus.Deleted
                        select new { employeeeducationaldetail = s , user = t };


            var totalCount = await _dataContext.EmployeeEducationalDetails.CountAsync(s => s.Status != Constants.RecordStatus.Deleted);


            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(s => s.employeeeducationaldetail.Education.Contains(search) ||
                                          s.employeeeducationaldetail.NameOfBoardOrUniversity.Contains(search));
                                        
            }

            if(page > 0 && pageSize > 0)
            {
                query = query.Skip((page - 1) * pageSize).Take(pageSize);
            }

            query = query.OrderByDescending(s => s.employeeeducationaldetail.Id);   // orderbyDescending


            var data = await query.Select(s => new EmployeeEducationalDetailDetailDto
            {
                Id = s.employeeeducationaldetail.Id,
                UserId = s.employeeeducationaldetail.UserId,
                UserName = s.user.FirstName + " " + s.user.LastName,
                Education = s.employeeeducationaldetail.Education,
                NameOfBoardOrUniversity = s.employeeeducationaldetail.NameOfBoardOrUniversity,
                YearOfCompletion = s.employeeeducationaldetail.YearOfCompletion,
                MarksObtained = s.employeeeducationaldetail.MarksObtained,
                GPA = s.employeeeducationaldetail.GPA,
                CreatedOn = s.employeeeducationaldetail.CreatedOn,
                UpdatedOn = s.employeeeducationaldetail.UpdatedOn,
                Status = s.employeeeducationaldetail.Status,
            }).AsNoTracking().ToListAsync();



            return (data, totalCount);
        }



        public async Task<EmployeeEducationalDetailDetailDto> GetByIdAsync(int id)
        {
          return  await (from s in _dataContext.EmployeeEducationalDetails
                         join t in _dataContext.User on s.UserId equals t.Id
                         where s.Status != utilities.Constants.RecordStatus.Deleted && s.Id==id

                   select new EmployeeEducationalDetailDetailDto
                   {
                       Id = s.Id,
                       UserId = s.UserId,
                       UserName = t.FirstName + " " + t.LastName,
                       Education = s.Education,
                       NameOfBoardOrUniversity = s.NameOfBoardOrUniversity,
                       YearOfCompletion = s.YearOfCompletion,
                       MarksObtained = s.MarksObtained,   
                       GPA = s.GPA,
                       CreatedOn = s.CreatedOn,
                       UpdatedOn = s.UpdatedOn,
                       Status = s.Status,
                   }).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var data = await _dataContext.EmployeeEducationalDetails.Where(s=>s.Id==id).FirstOrDefaultAsync();
            data.Status=utilities.Constants.RecordStatus.Deleted;
            _dataContext.Update(data);
            await _dataContext.SaveChangesAsync();
        }

    }
}