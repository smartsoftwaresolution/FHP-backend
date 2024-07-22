using FHP.infrastructure.Manager.UserManagement;  
using FHP.infrastructure.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.services.NotificationServices
{
    public class NotificationService : INotificationService
    {
        private readonly ISendNotificationService _sendNotificationService;
        private readonly IFCMTokenManager _fCMTokenManager;

        public NotificationService(IFCMTokenManager fCMTokenManager,
                                   ISendNotificationService sendNotificationService)
        {
            _fCMTokenManager = fCMTokenManager;
            _sendNotificationService = sendNotificationService;
        }

        public async Task SendContractNotificationAsync()
        {
            var adminToken = await _fCMTokenManager.FcmTokenByRole("admin");
            var token = adminToken.OrderByDescending(a => a.Id).FirstOrDefault();

            if (token != null)
            {
                string adminMessage = "Hello, A contract has been created and singed by employer.";
                await _sendNotificationService.SendNotification("Contract created", adminMessage, token.TokenFCM);
            }

            var employeeToken = await _fCMTokenManager.FcmTokenByRole("employee");
            var tokens = employeeToken.OrderByDescending(e => e.Id).FirstOrDefault();

            if (tokens != null)
            {
                string employeeMessage = "Hello, A contract has been created. please signed contract for further process.";
                await _sendNotificationService.SendNotification("Contract created", employeeMessage, tokens.TokenFCM);
            }
        }
    }
}
