﻿using FHP.dtos.FHP.EmployeeProfessionalDetail;
using FHP.entity.FHP;
using FHP.factories.FHP;
using FHP.infrastructure.Repository.FHP;
using FHP.utilities;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.datalayer.Repository.FHP
{
    public class EmployeeProfessionalDetailRepository: IEmployeeProfessionalDetailRepository
    {
        private readonly DataContext _dataContext;

        public EmployeeProfessionalDetailRepository(DataContext dataContext)
        {
            _dataContext= dataContext;
        }

        public async Task AddAsync(EmployeeProfessionalDetail entity)
        {
            await _dataContext.EmployeeProfessionalDetails.AddAsync(entity);
            await _dataContext.SaveChangesAsync();
        }

        public void Edit(EmployeeProfessionalDetail entity)
        {
            _dataContext.EmployeeProfessionalDetails.Update(entity);
            _dataContext.SaveChanges();
        }


        public async Task<EmployeeProfessionalDetail> GetAsync(int id)
        {
            return await _dataContext.EmployeeProfessionalDetails.Where(s => s.Id == id).FirstOrDefaultAsync();
        }



        public async Task<(List<EmployeeProfessionalDetailDto> employeeProfessionalDetail, int totalCount)> GetAllAsync(int page, int pageSize,int userId, string? search, string? jobDescription, string? designation, string? yearOfExperience)
        {
            var query = from s in _dataContext.EmployeeProfessionalDetails
                        where s.Status != Constants.RecordStatus.Deleted
                        select new { employeeProfessionalDetail = s  };


            if(jobDescription != null)
            {
                query = query.Where(s => s.employeeProfessionalDetail.JobDescription == jobDescription);
            }

            if(designation != null)
            {
                query = query.Where(s => s.employeeProfessionalDetail.Designation == designation);  
            }

            if(yearOfExperience != null)
            {
                query = query.Where(s => s.employeeProfessionalDetail.YearsOfExperience == yearOfExperience);
            }

            if (userId > 0)
            {
                query = query.Where(s => s.employeeProfessionalDetail.UserId == userId);
            }

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(s => s.employeeProfessionalDetail.CompanyName.Contains(search) ||
                                       s.employeeProfessionalDetail.CompanyLocation.Contains(search) ||
                                       s.employeeProfessionalDetail.Designation.Contains(search) ||
                                       s.employeeProfessionalDetail.EmploymentStatus.Contains(search));
            }


            var totalCount = await query.CountAsync();

            query = query.OrderByDescending(s => s.employeeProfessionalDetail.Id);


            if (page > 0 && pageSize > 0)
            {
                query = query.Skip((page - 1) * pageSize).Take(pageSize); 
            }


            var data =await query.Select(s=> new EmployeeProfessionalDetailDto
            {
               Id=s.employeeProfessionalDetail.Id,
               UserId=s.employeeProfessionalDetail.UserId,
               JobDescription=s.employeeProfessionalDetail.JobDescription,
               StartDate=s.employeeProfessionalDetail.StartDate,
               EndDate=s.employeeProfessionalDetail.EndDate,
               CompanyName=s.employeeProfessionalDetail.CompanyName,
               CompanyLocation=s.employeeProfessionalDetail.CompanyLocation,
               Designation=s.employeeProfessionalDetail.Designation,
               EmploymentStatus=s.employeeProfessionalDetail.EmploymentStatus,
               YearsOfExperience=s.employeeProfessionalDetail.YearsOfExperience,
               CreatedOn=s.employeeProfessionalDetail.CreatedOn,
               UpdatedOn=s.employeeProfessionalDetail.UpdatedOn,
               Status=s.employeeProfessionalDetail.Status,
            }).AsNoTracking().ToListAsync();

            return (data, totalCount);
                       
        }

        public async Task<EmployeeProfessionalDetailDto> GetByIdAsync(int id)
        {
            return await (from s in _dataContext.EmployeeProfessionalDetails
                          where s.Status != utilities.Constants.RecordStatus.Deleted &&
                          s.Id == id

                          select new EmployeeProfessionalDetailDto
                          {
                              Id = s.Id,
                              UserId = s.UserId,
                              JobDescription = s.JobDescription,
                              StartDate = s.StartDate,
                              EndDate = s.EndDate,
                              CompanyName = s.CompanyName,
                              CompanyLocation = s.CompanyLocation,
                              Designation = s.Designation,
                              EmploymentStatus = s.EmploymentStatus,
                              YearsOfExperience = s.YearsOfExperience,
                              CreatedOn = s.CreatedOn,
                              UpdatedOn = s.UpdatedOn,
                              Status = s.Status,
                          }).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var data = await _dataContext.EmployeeProfessionalDetails.Where(s => s.Id == id).FirstOrDefaultAsync();
            data.Status = Constants.RecordStatus.Deleted;
            _dataContext.Update(data);
            await _dataContext.SaveChangesAsync();
        }
    }
}
