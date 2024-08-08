using FHP.models.FHP.Contract;
using FHP.models.FHP.EmployeeAvailability;
using FHP.models.FHP.JobPosting;
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
        Task SendJobPostingNotifcationAsync(AddJobPostingModel model);
        Task AddUserRegistrationNotificationAsync(AddUserModel model);
        Task EditContractNotificationAsync(AddContractModel model);
        Task JobRequestNotificationAsync();
        Task EmployeeAcceptJobRequestNotificationAsync(SetEmployeeAvailabilityModel model);
        Task ShortlistNotificationAsync();
        
    }
}
