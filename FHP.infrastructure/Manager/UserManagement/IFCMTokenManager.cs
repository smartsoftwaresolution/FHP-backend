using FHP.dtos.UserManagement.FcmToken;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.infrastructure.Manager.UserManagement
{
    public interface IFCMTokenManager
    {
        Task<List<FcmTokenDetailDto>> FcmTokenByRole(string roleName);
    }
}
