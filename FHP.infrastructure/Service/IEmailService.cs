using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.infrastructure.Service
{
    public interface IEmailService
    {
        Task SendverificationEmail(string email,int userId, string origin);

        Task SendContractEmail(string email,string employerEmail,int userId, int employerId,string htmlBody,string subject);

       
    }
}
