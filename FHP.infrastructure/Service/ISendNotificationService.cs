using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.infrastructure.Service
{
    public interface ISendNotificationService
    {
        Task<bool> SendNotification(string title, string body, string token);
    }
}
