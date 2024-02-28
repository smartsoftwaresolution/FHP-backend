using DocumentFormat.OpenXml.ExtendedProperties;
using FHP.infrastructure.Manager.UserManagement;
using FHP.infrastructure.Service;
using FHP.models.UserManagement;
using FHP.services;
using FHP.utilities;
using Google.Apis.Gmail.v1.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FHP.Controllers.UserManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserManager _manager;
        private readonly IExceptionHandleService _exceptionHandleService;
        private readonly IEmailService _emailService;
        private readonly IFileUploadService _fileUploadService;
        public UserController(IUserManager manager,
                              IExceptionHandleService exceptionHandleService,
                              IEmailService emailService,
                              IFileUploadService  fileUploadService
                              )
        {
            _manager = manager;
            _exceptionHandleService = exceptionHandleService;
            _emailService = emailService;
            _fileUploadService = fileUploadService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddAsync(AddUserModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }

            var response = new BaseResponseAdd();

            try
            {

                if (model.Id == 0 &&
                    !string.IsNullOrEmpty(model.RoleName) &&
                    !string.IsNullOrEmpty(model.Email) &&
                    !string.IsNullOrEmpty(model.Password))
                {
                    int userid = 0;
                    userid = await _manager.AddAsync(model);
                    await _emailService.SendverificationEmail(model.Email, userid);

                    response.StatusCode = 200;
                    response.Message = Constants.added;

                    return Ok(response);
                }

                response.StatusCode = 400;
                response.Message = Constants.provideValues;
                return BadRequest(response);

            }
            catch (Exception ex)
            {
                return await _exceptionHandleService.HandleException(ex);
            }
        }



        [HttpPut("edit")]
        public async Task<IActionResult> EditAsync(AddUserModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }

            var response = new BaseResponseAdd();


            try
            {
                if (model.Id >= 0 && model != null)
                {
                    await _manager.EditAsync(model);
                    response.StatusCode = 200;
                    response.Message = Constants.updated;
                    return Ok(response);
                }

                response.StatusCode = 400;
                response.Message = Constants.provideValues;
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                return await _exceptionHandleService.HandleException(ex);
            }
        }

        [HttpGet("getall-pagination")]
        public async Task<IActionResult> GetAllAsync(int page, int pageSize, string? search, string? roleName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }

            var response = new BaseResponsePagination<object>();


            try
            {
                var data = await _manager.GetAllAsync(page, pageSize, search, roleName);
                if (data.user != null)
                {
                    response.StatusCode = 200;
                    response.Data = data.user;
                    response.TotalCount = data.totalCount;
                    return Ok(response);
                }

                response.StatusCode = 400;
                response.Message = Constants.error;
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                return await _exceptionHandleService.HandleException(ex);
            }

        }

        [HttpGet("getbyid")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }

            var response = new BaseResponseAddResponse<object>();

            try
            {


                var data = await _manager.GetByIdAsync(id);
                if (data != null)
                {
                    response.StatusCode = 200;
                    response.Data = data;
                    return Ok(response);
                }

                response.StatusCode = 400;
                response.Message = Constants.error;
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                return await _exceptionHandleService.HandleException(ex);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }

            var response = new BaseResponseAdd();

            try
            {
                if (id <= 0)
                {
                    response.StatusCode = 400;
                    response.Message = "ID Required";
                    return BadRequest(response);
                }
                await _manager.DeleteAsync(id);
                response.StatusCode = 200;
                response.Message = Constants.deleted;
                return Ok(response);
            }
            catch (Exception ex)
            {
                return await _exceptionHandleService.HandleException(ex);
            }
        }


        [HttpPatch("verify-user")]
        public async Task<IActionResult> VerifyUserAsync(int userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }

            var response = new BaseResponseAdd();
            try
            {
                if(userId > 0)
                {
                    await _manager.VerifyUser(userId);
                    response.StatusCode = 200;
                    response.Message = "User Verified Successfully!!";
                    return Ok(response);
                }
                response.StatusCode = 400;
                response.Message = Constants.provideValues;
                return BadRequest(response);
            }
            catch(Exception ex)
            {
                return await _exceptionHandleService.HandleException(ex);

            }
        }


        [HttpPatch("update-Profile-pic")]
        public async Task<IActionResult> AddProfilePictureAsync(int userId, IFormFile? picUrl,string roleName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }

            var response = new BaseResponseAdd();
            try
            {
                if (userId < 0)
                {
                    response.StatusCode = 400;
                    response.Message = Constants.provideValues;
                    return BadRequest(response);
                }
                string pic = string.Empty;
                if(picUrl != null)
                {
                    pic = await _fileUploadService.UploadIFormFileAsync(picUrl);
                }
                await _manager.AddUserPic(userId,pic,roleName);
                response.StatusCode = 200;
                response.Message = Constants.added;
                return Ok(response);
            }
            catch (Exception ex)
            {
                return await _exceptionHandleService.HandleException(ex);

            }
        }
    }
}
