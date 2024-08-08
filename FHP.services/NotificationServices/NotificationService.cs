using FHP.infrastructure.Manager.UserManagement;  
using FHP.infrastructure.Service;
using FHP.models.FHP.Contract;
using FHP.models.FHP.EmployeeAvailability;
using FHP.models.FHP.JobPosting;
using FHP.models.UserManagement.User;
using FHP.utilities;

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

        public async Task AddUserRegistrationNotificationAsync(AddUserModel model)
        {
            var tokens = await _fCMTokenManager.FcmTokenByRole("admin");

            // Check if tokens exist
            if (tokens.Any())
            {
                string body = "";
                string Title = "";

                if (model.RoleName.ToLower().Contains("employee"))
                {
                    body = "A new employee has joined the platform.Kindly review their information and greet them warmly";
                    
                    Title = "A new employee has joined";
                }

                else if (model.RoleName.ToLower().Contains("employer"))
                {
                    body = "A new employer has joined the platform.Kindly review their information and greet them warmly";
                    Title = "A new employer has joined";
                }

                // Send notification using the first token found in the list
                var token = tokens.OrderByDescending(e => e.Id).FirstOrDefault();

                // var token = tokens.FirstOrDefault();
                if (token != null)
                {
                    await _sendNotificationService.SendNotification(Title, body, token.TokenFCM);
                }
            }
        }

        public async Task EditContractNotificationAsync(AddContractModel model)
        {
            if(model.IsSignedByEmployee == true)
            {
                var adminToken1 = await _fCMTokenManager.FcmTokenByRole("admin");
                var token1 = adminToken1.OrderByDescending(e => e.Id).FirstOrDefault();

                if(token1 != null) 
                {
                    string adminMsg = "A contract has been signed by employee.";
                    await _sendNotificationService.SendNotification("contract signed", adminMsg, token1.TokenFCM);
                }

                var employerToken1 = await _fCMTokenManager.FcmTokenByRole("employer");
                var token2 = employerToken1.OrderByDescending(e => e.Id).FirstOrDefault();  

                if(token2 != null)
                {
                    string empMsg = "A contract has been signed by employee.";
                    await _sendNotificationService.SendNotification("contract signed", empMsg, token2.TokenFCM);
                }
            }


            if (model.IsSignedByEmployer == true)
            {
                var adminToken1 = await _fCMTokenManager.FcmTokenByRole("admin");
                var token1 = adminToken1.OrderByDescending(e => e.Id).FirstOrDefault();

                if (token1 != null)
                {
                    string adminMsg = "A contract has been signed by employer.";
                    await _sendNotificationService.SendNotification("contract signed", adminMsg, token1.TokenFCM);
                }

                var employeeToken1 = await _fCMTokenManager.FcmTokenByRole("employee");
                var token2 = employeeToken1.OrderByDescending(e => e.Id).FirstOrDefault();

                if (token2 != null)
                {
                    string empMsg = "A contract has been signed by employer.";
                    await _sendNotificationService.SendNotification("contract signed", empMsg, token2.TokenFCM);
                }
            }

        }



        public async Task EmployeeAcceptJobRequestNotificationAsync(SetEmployeeAvailabilityModel model)
        {
            var adminToken = await _fCMTokenManager.FcmTokenByRole("admin");
            var token = adminToken.OrderByDescending(a => a.Id).FirstOrDefault();

            if (model.EmployeeAvailability == Constants.EmployeeAvailability.Available)
            {
                if (token != null)
                {
                    string adminMessage = "An employee is succesfully accepted job requested for this job.";
                    await _sendNotificationService.SendNotification("Job request accepted", adminMessage, token.TokenFCM);
                }
            }
        }

        public async Task JobRequestNotificationAsync()
        {
            var admintoken = await _fCMTokenManager.FcmTokenByRole("employee");
            var token = admintoken.OrderByDescending(e => e.Id).FirstOrDefault();

            if (token != null)
            {
                string employeeMessage = "Congratulation you have received job request.";
                await _sendNotificationService.SendNotification("Job request.", employeeMessage, token.TokenFCM);
            }

        }

        public async Task SendContractNotificationAsync()
        {
                var employeeToken = await _fCMTokenManager.FcmTokenByRole("employee");
                var tokens = employeeToken.OrderByDescending(s => s.Id).FirstOrDefault();

                if (tokens != null)
                {
                    string employeeMessage = "A contract has been created. please signed contract for further process.";
                    await _sendNotificationService.SendNotification("Contract", employeeMessage, tokens.TokenFCM);
                }

                var employerToken = await _fCMTokenManager.FcmTokenByRole("employer");
                var token = employerToken.OrderByDescending(s => s.Id).FirstOrDefault();

                if (token != null)
                {
                    string employerMessage = "A contract has been created. please signed contract for further process.";
                    await _sendNotificationService.SendNotification("Contract", employerMessage, token.TokenFCM);
                }

        }

        public async Task SendJobPostingNotifcationAsync(AddJobPostingModel model)
        {

            var admintoken = await _fCMTokenManager.FcmTokenByRole("admin");
            var token = admintoken.OrderByDescending(e => e.Id).FirstOrDefault();

            if (model.JobPosting == Constants.JobPosting.Submitted)
            {
                if (token != null)
                {
                    string body = "Dear Admin,A new job post has been created.Please review the details and take any necessary actions.";
                    await _sendNotificationService.SendNotification("New Job Post ", body, token.TokenFCM);
                }
            }
        }

        public async Task ShortlistNotificationAsync()
        {
            var employeetoken = await _fCMTokenManager.FcmTokenByRole("employee");
            var token = employeetoken.OrderByDescending(e => e.Id).FirstOrDefault();

            if (token != null)
            {
                string message = "Congratulation you are shortlisted for the job.";

                await _sendNotificationService.SendNotification("Shortlisted", message, token.TokenFCM);
            }

            var employerToken = await _fCMTokenManager.FcmTokenByRole("employer");
            var tokens = employerToken.OrderByDescending(e => e.Id).FirstOrDefault();

            if (tokens != null)
            {
                string msg = "candidate is shortlisted";
                await _sendNotificationService.SendNotification("shortlist", msg, tokens.TokenFCM);
            }
        }
    }
}
