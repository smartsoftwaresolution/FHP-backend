using FHP.infrastructure.Manager.FHP;
using FHP.infrastructure.Repository.FHP;

namespace FHP.manager.FHP
{
    public class EmployerDashboardManager : IEmployerDashboardManager
    {
        private readonly IEmployerDashboardRepository _empRepository;
        public EmployerDashboardManager(IEmployerDashboardRepository empRepository)
        {
            _empRepository = empRepository;
        }

        
        public async Task<int> GetAllDraftPost()
        {
            return await _empRepository.GetAllDraftPost();
        }

        public async Task<int> GetAllJobPost()
        {
          return await _empRepository.GetAllJobPost();
        }

        public async Task<int> GetAllContract(int id)
        {
            return await _empRepository.GetAllContract(id);
        }

        public async Task<int> TotalJobReq(int employeeId)
        {
            return await _empRepository.TotalJobReq(employeeId);
        }
    }
}
