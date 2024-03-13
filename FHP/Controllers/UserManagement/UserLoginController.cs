
using FHP.entity.UserManagement;
using FHP.infrastructure.Manager.UserManagement;
using FHP.infrastructure.Service;
using FHP.models.UserManagement.UserLogin;
using FHP.utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;

namespace FHP.Controllers.UserManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLoginController : ControllerBase
    {
        private readonly IUserManager _manager;
        private readonly IExceptionHandleService _exceptionHandleService;
        private readonly IConfiguration _configuration;
        private readonly IEmailSettingManager _emailSettingManager;

        public UserLoginController(IUserManager manager,
                                   IExceptionHandleService exceptionHandleService,
                                   IConfiguration configuration,
                                   IEmailSettingManager emailSettingManager)
        {
            _manager = manager;
            _exceptionHandleService = exceptionHandleService;
            _configuration = configuration;
            _emailSettingManager = emailSettingManager;
        }

        
        [HttpPost("userlogin-email")] // API Endpoint for user login by email
        public async Task<IActionResult> UserLoginAsync(UserLoginModel model)
        {
            // Checks if the model state is valid
            if (!ModelState.IsValid)
            {
                // Returns a BadRequest response with a list of errors if model state is not valid
                return BadRequest(ModelState.GetErrorList());
            }

         
            // Initializes the response object for returning the result
            var response = new BaseResponse<object>();

            try
            {
                // Retrieves user data by email
                var data = await _manager.GetUserByEmail(model.Email);

                // Checks if user data is not null
                if (data != null)
                    {

                       // Checks if the user account is inactive
                    if (data.Status == Constants.RecordStatus.Inactive)
                        {
                            response.StatusCode = 400;
                            response.Message = "account is inactive";
                            return BadRequest(response);
                        }

                       // Validates the password
                    if (!string.IsNullOrEmpty(data.Password) && utilities.Utility.Decrypt(model.Password, data.Password) == false)
                        {
                            response.Message = "Invalid password. Please enter your current valid password.";
                            response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            return BadRequest(response);
                        }

                    // Checks if email is verified
                    if (data.IsVerify == null || data.IsVerify == false)
                        {
                            response.Message = "email is send to your account,Plz verify the account first";
                            response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            return BadRequest(response);
                        }


                         // Creates JWT token for authentication
                        var tokenHandler = new JwtSecurityTokenHandler();
                        var key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("Jwt:secret"));
                        var tokenDescription = new SecurityTokenDescriptor
                        {
                            Subject = new ClaimsIdentity(new[]
                                    {
                                   new Claim("id", data.Id.ToString()),
                                   new Claim("Email", data.Email.ToString()),
                                   new Claim("RoleId", data.RoleId.ToString()),
                                   new Claim("FirstName", data.FirstName.ToString()),
                                   new Claim("RoleName", data.RoleName.ToString()),
                               }),

                            Audience = _configuration.GetValue<string>("Jwt:Audience"),
                            Issuer = _configuration.GetValue<string>("Jwt:Issuer"),
                            Expires = DateTime.UtcNow.AddDays(1),
                            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                        };

                        var token = tokenHandler.CreateToken(tokenDescription); 

                         // Logs user login
                        LoginModule login = new LoginModule();
                        login.CreatedOn = DateTime.UtcNow;
                        login.UserId = data.Id;
                        login.RoleId  = data.RoleId;
                      
                      
                        await _manager.UserLogIn(login); //userLogOn


                        // Sets response status code and message
                        response.StatusCode = (int)HttpStatusCode.OK;
                        response.Message = "User logged in Successfully!!";
                        response.Data = tokenHandler.WriteToken(token);
                        return Ok( response);
                    }

                    else
                    {
                        // Returns a response indicating invalid email
                        response.StatusCode = 400;
                        response.Message = "Invalid Email";
                        return BadRequest(response);
                    }

            }

            catch (Exception ex)
            {
                // Handle the exception using the provided exception handling service.
                return await _exceptionHandleService.HandleException(ex); 
            }
        }

        
        [HttpPost("userlogin-governmentid")] // API Endpoint for user login by governmentId
        public async Task<IActionResult> UserLoginByGovId(UserLoginGovIdModel model)
        {
                // Checks if the model state is valid
            if (!ModelState.IsValid)
            {
                // Returns a BadRequest response with a list of errors if model state is not valid
                return BadRequest(ModelState.GetErrorList());
            }

            // Initializes the response object for returning the result
            var response = new BaseResponse<object>();

            try
            {
               
                   var data = await _manager.GetUserByGovernmentId(model.GovernmentId);


                // Checks if user data is not null
                if (data != null)
                    {
                        // Checks if the user account is inactive
                        if (data.Status == Constants.RecordStatus.Inactive)
                        {
                            response.StatusCode = 400;
                            response.Message = "account is inactive";
                            return BadRequest(response);
                        }
                        
                        if (!string.IsNullOrEmpty(model.Password) && utilities.Utility.Decrypt(model.Password, data.Password) == false)
                        {
                            response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            response.Message = "Invalid password. Please enter your current valid password. ";
                            return BadRequest(response);
                        }
                        if (data.IsVerify == null || data.IsVerify == false)
                        {
                            response.Message = "email is send to your account,Plz verify the account first";
                            response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            return BadRequest(response);
                        }

                         // Creates JWT token for authentication
                          var tokenHandler =  new JwtSecurityTokenHandler();
                          var key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("Jwt:secret"));

                          var tokenDescription = new SecurityTokenDescriptor
                          {
                            Subject = new ClaimsIdentity(new[]
                            {
                                new Claim("id", data.Id.ToString()),
                                new Claim("GovernmentId", data.GovernmentId.ToString()),
                                new Claim("RoleId", data.RoleId.ToString()),
                                new Claim("RoleName",data.RoleName.ToString()),
                                new Claim("FirstName", data.FirstName.ToString()),
                               
                            }),

                            Audience = _configuration.GetValue<string>("Jwt:Audience"),
                            Issuer=_configuration.GetValue<string>("Jwt:Issuer"),
                            Expires=DateTime.UtcNow.AddDays(1),
                            SigningCredentials=new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature)
                          };

                          var token = tokenHandler.CreateToken(tokenDescription); //tokenHandler


                             // Logs user login
                            LoginModule login = new LoginModule();
                            login.CreatedOn = DateTime.UtcNow;
                            login.UserId = data.Id;
                            login.RoleId = data.RoleId;
                       

                         await _manager.UserLogIn(login);

                    // Sets response status code and message
                        response.StatusCode = (int)HttpStatusCode.OK;
                        response.Message = "User logged in Successfully!!";
                        response.Data = tokenHandler.WriteToken(token);
                        return Ok(response);
                    }
                    else
                    {
                    // Returns a response indicating invalid governmentId
                        response.StatusCode = 400;
                        response.Message = "Invalid Government ID";
                        return BadRequest(response);
                    }
               
            }
            catch(Exception ex)
            {
                // Handle the exception using the provided exception handling service.
                return await _exceptionHandleService.HandleException(ex);   
            }
        }


        
        [HttpPost("logout")] // Endpoint for user logout
        public async Task<IActionResult> UserLogOutAsync(int userId)
        {
            // Checks if the model state is valid
            if (!ModelState.IsValid)
            {
                // Returns a BadRequest response with a list of errors if model state is not valid
                return BadRequest(ModelState.GetErrorList());
            }


            // Initializes the response object for returning the result
            var response = new BaseResponse<object>();

            try
            {
                // Checks if a valid user ID is provided
                if (userId >=0)
                {
                    // Calls the manager to log out the user asynchronously
                    await _manager.UserLogOut(userId);

                    // Sets StatusCode to 200 indicating success
                    response.StatusCode= (int)HttpStatusCode.OK;
                    response.Message = "User logged out Sucessfully. ";

                    // Returns Ok response with the success message
                    return Ok(response);
                }

                response.StatusCode = 400;
                response.Message = "User Id must greater than zero.";

                // Returns BadRequest response with the error message
                return BadRequest(response);
            }

            catch(Exception ex)
            {
                // Handle the exception using the provided exception handling service.
                return await _exceptionHandleService.HandleException(ex); 
            }
        }


        
        [HttpPatch("change-password")] //  API Endpoint for changing user password
        public async Task<IActionResult> ChangePasswordAsync(int userId, string password)
        {
            // Checks if the model state is valid
            if (!ModelState.IsValid)
            {
                // Returns a BadRequest response with a list of errors if model state is not valid
                return BadRequest(ModelState.GetErrorList());
            }

            // Initializes the response object for returning the result
            var response = new BaseResponseAdd();

            try
            {
                // Checks if a valid user ID is provided
                if (userId > 0)
                {
                    // Calls the manager to change user password asynchronously
                    await _manager.ChangePassword(userId, password);

                    // Sets StatusCode to 200 indicating success
                    response.StatusCode = 200;
                    response.Message = $"Password changed Successfully!!";

                    // Returns Ok response with the success message
                    return Ok(response);
                }

                response.StatusCode = 400;
                response.Message = Constants.provideValues;

                // Returns BadRequest response with the error message
                return BadRequest(response);

            }
            catch (Exception ex)
            {
                // Handle the exception using the provided exception handling service.
                return await _exceptionHandleService.HandleException(ex);
            }
        }


       
        [HttpPatch("forgot-password")] //  API Endpoint for handling forgot password request
        public async Task<IActionResult> ForgotPasswordAsync( string email)
        {
            // Checks if the model state is valid
            if (!ModelState.IsValid)
            {
                // Returns a BadRequest response with a list of errors if model state is not valid
                return BadRequest(ModelState.GetErrorList());
            }

            // Initializes the response object for returning the result
            var response = new BaseResponseAdd();


            try
            {
                
                // Retrieves user data by email
                var exist = await _manager.GetUserByEmail(email);

                // Checks if user data exists
                if (exist == null)
                {
                    // Sets StatusCode to 400 indicating a bad request
                    response.StatusCode = 400;
                    response.Message = $"email does not exist";

                    // Returns BadRequest response with the error message
                    return BadRequest(response);
                }

                // Generates a random OTP
                Random generator = new Random();
                String r = generator.Next(0, 1000000).ToString("D6"); 

                // Saves OTP for the user
                bool result = await _manager.SaveOtp(email,Convert.ToInt32(r)); 

                if (result == true)
                {
                    var data = new
                    {
                        otp = Convert.ToInt32(r),
                        msg = "OTP send"
                    };

                    var emailSeting = await _emailSettingManager.GetByIdAsync(1);

                    // Constructs email message
                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient();

                    mail.From = new MailAddress(emailSeting.Email, "FHP"); // Sender email address
                    mail.To.Add(email);
                    mail.Subject = "Reset Password";
                    mail.Body = "<Html>"
                                 + "<head>" +

                                 "</head>" +
                                 "<body>" +
                                 "<div style='background: #f2f3f8; padding: 50px 0'>" +
                                 "<div style = 'max-width:670px;background:#fff; border-radius:3px; text-align:center;-webkit-box-shadow:0 6px 18px 0 rgba(0,0,0,.06);-moz-box-shadow:0 6px 18px 0 rgba(0,0,0,.06);box-shadow:0 6px 18px 0 rgba(0,0,0,.06);width: 450px; margin:0 auto;background: #fff; padding: 20px;'>" +
                                 "<h1 style='color:#1e1e2d; font-weight:500;font-size:32px;font-family:'Rubik',sans-serif;text-align:center;'>You have requested to reset your password</h1>" +
                                 "<p style='text - align:center; font - family:'Rubik',sans - serif; '>Follow the instructions to reset your password</p>" +
                                 " <p style='text - align:center; font - family:'Rubik',sans - serif; '>Please use the otp to reset your password : <strong><span>" +
                                 +data.otp +
                                 "</span></strong></p>" +
                                 "</div>" +
                                 "</div>" +
                                 "</body>" +
                            "</Html>";
                    mail.IsBodyHtml = true;
                    SmtpServer.Host = "smtp.gmail.com";
                    SmtpServer.Port = 587;

                    SmtpServer.UseDefaultCredentials = false;
                    SmtpServer.Credentials = new System.Net.NetworkCredential(emailSeting.Email, emailSeting.AppPassword);
                    SmtpServer.EnableSsl = true;
                    SmtpServer.Send(mail);

                    // Sets StatusCode to 200 indicating success
                    response.StatusCode = 200;
                    response.Message = "otp send to your mail";

                    // Returns Ok response with the success message
                    return Ok(response);
                }
                else
                {
                    // Sets StatusCode to 400 indicating a bad request
                    response.StatusCode = 400;
                    response.Message = "email not found";

                    // Returns BadRequest response with the error message
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                // Handles exceptions and returns appropriate response
                return await _exceptionHandleService.HandleException(ex);
            }
        }

        
        [HttpPatch("verify-email-otp")] // API Endpoint for verifying email OTP
        public async Task<IActionResult> VerifyEmailOtpAsync(string email,int otp)
        {
            // Checks if the model state is valid
            if (!ModelState.IsValid)
            {
                // Returns a BadRequest response with a list of errors if model state is not valid
                return BadRequest(ModelState.GetErrorList());
            }

            // Initializes the response object for returning the result
            var response = new BaseResponse<object>();

            try
            {
                // Retrieves user data by email
                var exist = await _manager.GetUserByEmail(email);

                // Checks if user data exists
                if (exist == null)
                {
                    response.StatusCode = 400;
                    response.Message = $"email does not exist";

                    // Returns BadRequest response with the error message
                    return BadRequest(response);
                }


                // Compares the provided OTP with the OTP saved in the user's data
                if (exist.Otp == otp)
                {
                   
                    response.Data = exist.Id;

                    // Sets StatusCode to 200 indicating success
                    response.StatusCode = 200;
                    response.Message = $"otp matched successfully!!";

                    // Returns Ok response with the success message and user ID
                    return Ok(response);
                }

                response.StatusCode = 400;
                response.Message = $"otp not matched";

                // Returns BadRequest response with the error message
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                // Handles exceptions and returns appropriate response
                return await _exceptionHandleService.HandleException(ex);

            }
        }

    }
}
