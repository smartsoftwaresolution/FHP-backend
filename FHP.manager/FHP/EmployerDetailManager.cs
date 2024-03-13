using FHP.dtos.FHP;
using FHP.dtos.FHP.EmployerDetail;
using FHP.factories.FHP;
using FHP.infrastructure.Manager.FHP;
using FHP.infrastructure.Repository.FHP;
using FHP.models.FHP.EmployerDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.manager.FHP
{
    public class EmployerDetailManager : IEmployerDetailManager
    {
        private readonly IEmployerDetailRepository _repository;

        public EmployerDetailManager(IEmployerDetailRepository repository)
        {
            _repository=repository;
        }

        public async Task AddAsync(AddEmployerDetailModel model, string vatCertificate, string certificateRegistration)
        {
            await _repository.AddAsync(EmployerDetailFactory.Create(model,vatCertificate,certificateRegistration)); 
           // await _repository.AddAsync(model);
        }

        public async Task Edit(AddEmployerDetailModel model,string vatCertificate,string certificateRegistration)
        {
            var data = await _repository.GetAsync(model.Id);
            EmployerDetailFactory.Update(data, model,vatCertificate,certificateRegistration);
            _repository.Edit(data);
        }

        public async Task<(List<EmployerDetailDto> employerDetail, int totalCount)> GetAllAsync(int page, int pageSize,int userId, string? search)
        {
          return await _repository.GetAllAsync(page, pageSize,userId, search);
        }

        public async Task<EmployerDetailDto> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
