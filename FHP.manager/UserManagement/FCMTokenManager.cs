using FHP.dtos.UserManagement.FcmToken;
using FHP.infrastructure.Manager.UserManagement;
using FHP.infrastructure.Repository.UserManagement;

namespace FHP.manager.UserManagement
{
    public class FCMTokenManager : IFCMTokenManager
    {
        private readonly IFCMTokenRepository _repository;
        public FCMTokenManager(IFCMTokenRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<FcmTokenDetailDto>> FcmTokenByRole(string roleName)
        {
            return await _repository.FcmTokenByRole(roleName);
        }

    }
}
