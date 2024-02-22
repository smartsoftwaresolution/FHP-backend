﻿
using FHP.entity.UserManagement;
using FHP.infrastructure.Manager.UserManagement;
using FHP.infrastructure.Service;
using FHP.models.UserManagement.UserLogin;
using FHP.services;
using FHP.utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
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
        
        public UserLoginController(IUserManager manager,
                                   IExceptionHandleService exceptionHandleService,
                                   IConfiguration configuration)
        {
            _manager = manager;
            _exceptionHandleService = exceptionHandleService;
            _configuration = configuration;

        }


        [HttpPost("userlogin-email")]
        public async Task<IActionResult> UserLoginAsync(UserLoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return Ok(ModelState.GetErrorList());
            }

            var response = new BaseResponse<object>();

            try
            {
                    var data = await _manager.GetUserByEmail(model.Email);
                    if(data != null)
                    {
                        if (!string.IsNullOrEmpty(data.Password) && utilities.Utility.Decrypt(model.Password, data.Password) == false)
                        {
                            response.Message = "Invalid password. Please enter your current valid password.";
                            response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            return BadRequest(response);
                        }

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

                        LoginModule login = new LoginModule();
                        login.CreatedOn = DateTime.UtcNow;
                        login.UserId = data.Id;
                        login.RoleId  = data.RoleId;
                      
                      
                        await _manager.UserLogIn(login);

                        response.StatusCode = (int)HttpStatusCode.OK;
                        response.Message = "User logged in Successfully!!";
                        response.Data = tokenHandler.WriteToken(token);
                        return Ok( response);
                    }

                    else
                    {
                        response.StatusCode = 400;
                        response.Message = "Invalid Email";
                        return BadRequest(response);
                    }

                

               
            }

            catch (Exception ex)
            {
                return await _exceptionHandleService.HandleException(ex);
            }
        }


        [HttpPost("userlogin-governmentid")]
        public async Task<IActionResult> UserLoginByGovId(UserLoginGovIdModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }

           var response = new BaseResponse<object>();

            try
            {
               
                   var data = await _manager.GetUserByGovernmentId(model.GovernmentId);

                    if (data != null)
                    {

                        if(!string.IsNullOrEmpty(model.Password) && utilities.Utility.Decrypt(model.Password, data.Password) == false)
                        {
                            response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            response.Message = "Invalid password. Please enter your current valid password. ";
                            return BadRequest(response);
                        }

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

                        var token = tokenHandler.CreateToken(tokenDescription);

                        LoginModule login = new LoginModule();
                        login.CreatedOn = DateTime.UtcNow;
                        login.UserId = data.Id;
                        login.RoleId = data.RoleId;
                       

                        await _manager.UserLogIn(login);

                        response.StatusCode = (int)HttpStatusCode.OK;
                        response.Message = "User logged in Successfully!!";
                        response.Data = tokenHandler.WriteToken(token);
                        return Ok(response);
                    }
                    else
                    {
                        response.StatusCode = 400;
                        response.Message = "Invalid Government ID";
                        return BadRequest(response);
                    }
               
            }
            catch(Exception ex)
            {
                return await _exceptionHandleService.HandleException(ex);
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> UserLogOutAsync(int userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }

            var response = new BaseResponse<object>();

            try
            {
                var header = Request.Headers["CompanyId"];
                int companyId = Convert.ToInt32(header);

                if(userId >=0)
                {
                    await _manager.UserLogOut(userId);
                    response.StatusCode= (int)HttpStatusCode.OK;
                    response.Message = "User logged out Sucessfully. ";
                    return Ok(response);
                }

                response.StatusCode = 400;
                response.Message = "User Id must greater than zero.";
                return BadRequest(response);
            }

            catch(Exception ex)
            {
               return  await _exceptionHandleService.HandleException(ex);
            }
        }
    }
}