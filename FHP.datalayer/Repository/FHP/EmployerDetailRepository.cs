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
    public class EmployerDetailRepository : IEmployerDetailRepository
    {
        private readonly DataContext _dataContext;

        public EmployerDetailRepository(DataContext dataContext)
        {
            _dataContext= dataContext;
        }

        public async Task AddAsync(EmployerDetail entity)
        {
            await _dataContext.EmployerDetails.AddAsync(entity);
            await _dataContext.SaveChangesAsync();
        }

        public void Edit(EmployerDetail entity)
        {
            _dataContext.EmployerDetails.Update(entity);
            _dataContext.SaveChanges();
        }


        public async Task<EmployerDetail> GetAsync(int id)
        {
            return await _dataContext.EmployerDetails.Where(s => s.Id == id).FirstOrDefaultAsync();
        }



        public async Task<(List<EmployerDetailDetailDto> employerDetail, int totalCount)> GetAllAsync(int page, int pageSize,int userId, string? search)
        {
            var query = from s in _dataContext.EmployerDetails
                        where s.Status != utilities.Constants.RecordStatus.Deleted
                        select new { employerDetail = s };

            var totalCount = await _dataContext.EmployerDetails.CountAsync(s => s.Status != utilities.Constants.RecordStatus.Deleted);

            if(userId > 0)
            {
                query = query.Where(s => s.employerDetail.UserId == userId);
            }
            if (!string.IsNullOrEmpty(search))
            {
                query =query.Where(s=>
                                       s.employerDetail.NationalAddress.Contains(search) ||
                                       s.employerDetail.TypeOfBusiness.Contains(search) ||
                                       s.employerDetail.WebAddress.Contains(search));   
            }


            if(page > 0 && pageSize > 0)
            {
                query = query.Skip((page - 1) * pageSize).Take(pageSize);
            }

            query = query.OrderByDescending(s => s.employerDetail.Id);

            var data = await query.Select (s=> new EmployerDetailDetailDto
            {
                Id = s.employerDetail.Id,
                UserId = s.employerDetail.UserId,
                NationalAddress = s.employerDetail.NationalAddress,
                CertificateRegistrationURL = s.employerDetail.CertificateRegistrationURL,
                VATCertificateURL = s.employerDetail.VATCertificateURL,
                ContactId = s.employerDetail.ContactId,
                CityId = s.employerDetail.CityId,
                StateId = s.employerDetail.StateId,
                CountryId = s.employerDetail.CountryId,
                CompanyLogoURL = s.employerDetail.CompanyLogoURL,
                Telephone = s.employerDetail.Telephone,
                Fax = s.employerDetail.Fax,
                TypeOfBusiness = s.employerDetail.TypeOfBusiness,
                PrincipalBusinessActivity = s.employerDetail.PrincipalBusinessActivity,
                PersonToContact = s.employerDetail.PersonToContact,
                WebAddress = s.employerDetail.WebAddress,
                CreatedOn = s.employerDetail.CreatedOn,
                UpdatedOn = s.employerDetail.UpdatedOn,
                Status = s.employerDetail.Status,

            })
                .AsNoTracking()
                .ToListAsync();

            return (data, totalCount);
        }

        public async Task<EmployerDetailDetailDto> GetByIdAsync(int id)
        {
           return  await (from s in _dataContext.EmployerDetails
                   where s.Status != utilities.Constants.RecordStatus.Deleted
                   && s.Id == id  

                   select new EmployerDetailDetailDto
                   {
                       Id=s.Id,
                       UserId=s.UserId,
                       NationalAddress=s.NationalAddress,
                       CertificateRegistrationURL=s.CertificateRegistrationURL,
                       VATCertificateURL=s.VATCertificateURL,
                       ContactId=s.ContactId,   
                       CityId=s.CityId,
                       StateId=s.StateId,
                       CountryId=s.CountryId,
                       CompanyLogoURL=s.CompanyLogoURL,
                       Telephone=s.Telephone,
                       Fax=s.Fax,
                       TypeOfBusiness=s.TypeOfBusiness, 
                       PrincipalBusinessActivity=s.PrincipalBusinessActivity,   
                       PersonToContact=s.PersonToContact,
                       WebAddress=s.WebAddress, 
                       CreatedOn=s.CreatedOn,
                       UpdatedOn =s.UpdatedOn,
                       Status=s.Status, 
                   })
                   .AsNoTracking()
                   .FirstOrDefaultAsync();
                   
        }

        public async Task DeleteAsync(int id)
        {
            var data = await _dataContext.EmployerDetails.Where(s => s.Id == id).FirstOrDefaultAsync();
            data.Status = Constants.RecordStatus.Deleted;
            _dataContext.Update(data);
            await _dataContext.SaveChangesAsync();  
        }
    }
}
