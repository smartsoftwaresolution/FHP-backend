﻿using DocumentFormat.OpenXml.Bibliography;
using FHP.dtos.UserManagement;
using FHP.entity.UserManagement;
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
    public class UserManager : IUserManager
    {
        private readonly IUserRepository _repository;

        public UserManager(IUserRepository repository)
        {
            _repository=repository;
        }
        public async Task AddAsync(AddUserModel model)
        {
           await _repository.AddAsync(UserFactory.Create(model),model.RoleName);
        }
        public async Task EditAsync(AddUserModel model)
        {
            var data = await _repository.GetAsync(model.Id);
            UserFactory.Update(data,model);
            _repository.Edit(data);
        }

        public async Task<List<UserDetailDto>> GetAllAsync( )
        {
            return await _repository.GetAllAsync();
        }

        public async Task<UserDetailDto> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task DeleteAsync(int id)
        {

             await _repository.DeleteAsync(id);
        }


        public async Task<UserDetailDto> GetUserByEmail(string Email)
        {
           return  await _repository.GetUserByEmail(Email);
        }

        public async Task<UserDetailDto> GetUserByGovernmentId(string governmentId)
        {
          return   await _repository.GetUserByGovernmentId(governmentId);
        }

        public async Task UserLogIn(LoginModule entity)
        {
            await _repository.UserLogIn(entity);
        }

        public async Task UserLogOut(int userId)
        {
            await _repository.UserLogOut(userId);
        }
    }
}