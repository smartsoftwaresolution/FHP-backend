using FHP.models.UserManagement.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.infrastructure.Service
{
    public interface INotificationService
    {
        Task SendContractNotificationAsync();
    }
}
