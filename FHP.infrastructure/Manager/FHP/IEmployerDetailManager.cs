using FHP.dtos.FHP;
using FHP.dtos.FHP.EmployerDetail;
using FHP.models.FHP.EmployerDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.infrastructure.Manager.FHP
{
    public interface IEmployerDetailManager
    {
        Task AddAsync(AddEmployerDetailModel model,string vatCertificate,string certificateRegistration);
        Task Edit(AddEmployerDetailModel model,string vatCertificate,string certificateRegistration);
        Task<(List<EmployerDetailDto> employerDetail, int totalCount)> GetAllAsync(int page, int pageSize,int userId, string? search);
        Task<EmployerDetailDto> GetByIdAsync(int id);
        Task DeleteAsync(int id);
    }
}
