using FHP.dtos.FHP;
using FHP.models.FHP;

namespace FHP.infrastructure.Manager.FHP
{
    public interface IEmployeeEducationalDetailManager
    {
       Task AddAsync(AddEmployeeEducationalDetailModel model);
       Task Edit(AddEmployeeEducationalDetailModel model);
       Task<(List<EmployeeEducationalDetailDetailDto> employeeeducationaldetail, int totalCount)> GetAllAsync(int page,int pageSize,int userId,string? search);
       Task<EmployeeEducationalDetailDetailDto> GetByIdAsync(int id);
       Task DeleteAsync(int id);
    }
}
