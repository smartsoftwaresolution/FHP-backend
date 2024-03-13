using FHP.dtos.FHP;
using FHP.dtos.FHP.EmployeeEducationalDetail;
using FHP.models.FHP.EmployeeEducationalDetail;

namespace FHP.infrastructure.Manager.FHP
{
    public interface IEmployeeEducationalDetailManager
    {
       Task AddAsync(AddEmployeeEducationalDetailModel model);
       Task Edit(AddEmployeeEducationalDetailModel model);
       Task<(List<EmployeeEducationalDetailDto> employeeeducationaldetail, int totalCount)> GetAllAsync(int page,int pageSize,int userId,string? search);
       Task<EmployeeEducationalDetailDto> GetByIdAsync(int id);
       Task DeleteAsync(int id);
    }
}
