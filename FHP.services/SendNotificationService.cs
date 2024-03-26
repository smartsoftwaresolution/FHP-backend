using FHP.datalayer;
using FHP.infrastructure.Service;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace FHP.services
{
    public class SendNotificationService : ISendNotificationService
    {
        public readonly DataContext _dataContext;
        private readonly IHostingEnvironment _env;
        

        public SendNotificationService(DataContext dataContext, IHostingEnvironment env)
        {
            _dataContext = dataContext;
            _env = env;
        }

        public async Task<bool> SendNotification( string title, string body, string token)
        {
            try
            {
                var path = _env.ContentRootPath;
                path = path + "\\Auth.json";
                FirebaseApp app = null;
               // var data = await _dataContext.FCMTokens.Where(s => s.UserId == userId).ToListAsync();
                try
                {
                    app = FirebaseApp.Create(new AppOptions()
                    {
                        Credential = GoogleCredential.FromFile(path)
                    }, "rigup-dev");
                }
                catch(Exception ex)
                {
                    app = FirebaseApp.GetInstance("rigup-dev");
                }

                var fcm = FirebaseAdmin.Messaging.FirebaseMessaging.GetMessaging(app);
                


                    Message msg = new Message()
                    {
                        Notification = new Notification
                        {
                            Title = title,
                            Body = body,
                        },
                      //  Token = "dHno0BNkQRBBzbews07wGt:APA91bEQ2x3h9WotaqXv6jweFzYdfj-XNz4QRaf9r5eJZHzTwDkiP-a_iAdzu01dAibojLNsURS1oGRO1XALh-WskGrNufyiFB3B0zd-0hlaH7-NQgAAFm5DGKIokvCQhaHYV04nLuFN"
                      Token = token
                    };
                    await fcm.SendAsync(msg);

                
                return true;
            }

            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
