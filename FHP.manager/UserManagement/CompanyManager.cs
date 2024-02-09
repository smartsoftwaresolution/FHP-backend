using FHP.dtos.UserManagement;
using FHP.factories.UserManagement;
using FHP.infrastructure.Manager.UserManagement;
using FHP.infrastructure.Repository.UserManagement;
using FHP.models.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.manager.UserManagement
{
    public class CompanyManager : ICompanyManager
    {
        private ICompanyRepository _repository;

        public CompanyManager(ICompanyRepository repository)
        {
            _repository=repository;
        }
        public async Task  AddAsync(AddCompanyModel model)
        {
             await _repository.AddAsync(CompanyFactory.Create(model));   
        }

       
        public async Task EditAsync(AddCompanyModel model)
        {
            var data = await _repository.GetAsync(model.Id);
            CompanyFactory.Update(data, model);
             _repository.Edit(data);

        }

        public async Task<List<CompanyDetailDto>> GetAllAsync(int id)
        {
           return  await _repository.GetAllAsync(id);
        }


        public async Task<CompanyDetailDto> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }


        public async Task DeleteAsync(int id)
        {

            await _repository.DeleteAsync(id);
        }

    }
}
