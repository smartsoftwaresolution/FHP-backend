using FHP.dtos.UserManagement.Company;
using FHP.entity.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.infrastructure.Repository.UserManagement
{
    public interface ICompanyRepository
    {
        Task  AddAsync(Company entity);
        Task<Company> GetAsync(int id);
        void Edit(Company entity);
        Task<List<CompanyDetailDto>> GetAllAsync(int UserId);
        Task<CompanyDetailDto> GetByIdAsync(int Id);
        Task DeleteAsync(int id);
    }
}
