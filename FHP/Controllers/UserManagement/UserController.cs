
using FHP.infrastructure.DataLayer;
using FHP.infrastructure.Manager.UserManagement;
using FHP.infrastructure.Service;
using FHP.models.UserManagement;
using FHP.utilities;
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
        private readonly IUnitOfWork _unitOfWork;
        public UserController(IUserManager manager,
                              IExceptionHandleService exceptionHandleService,
                              IEmailService emailService,
                              IFileUploadService  fileUploadService,
                              IUnitOfWork unitOfWork
                              )
        {
            _manager = manager;
            _exceptionHandleService = exceptionHandleService;
            _emailService = emailService;
            _fileUploadService = fileUploadService;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddAsync(AddUserModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }

            var response = new BaseResponseAdd();
            await using var transaction = await _unitOfWork.BeginTransactionAsync();

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
                    await transaction.CommitAsync();
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
                await transaction.RollbackAsync();
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
            await using var transaction = await _unitOfWork.BeginTransactionAsync();


            try
            {
                if (model.Id >= 0 )
                {
                    await _manager.EditAsync(model);
                    await transaction.CommitAsync();
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
                await transaction.RollbackAsync();
                return await _exceptionHandleService.HandleException(ex);
            }
        }

        [HttpGet("getall-pagination")]
        public async Task<IActionResult> GetAllAsync(int page, int pageSize, string? search, string? roleName,bool isAscending)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }

            var response = new BaseResponsePagination<object>();


            try
            {
                var data = await _manager.GetAllAsync(page, pageSize, search, roleName,isAscending);
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
        public async Task<IActionResult> AddProfilePictureAsync(int userId, IFormFile? picUrl)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }
            await using var transaction = await _unitOfWork.BeginTransactionAsync();

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
                await _manager.AddUserPic(userId,pic);
                await transaction.CommitAsync();
                response.StatusCode = 200;
                response.Message = Constants.added;
                return Ok(response);
            }
            catch (Exception ex)
            {
                return await _exceptionHandleService.HandleException(ex);

            }
        }


        [HttpPatch("enable-disble-employee-employer")]
        public async Task<IActionResult> EnableDisableUserAsync(int userId,string roleName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }

            var response = new BaseResponseAdd();
            try
            {
                if (userId > 0)
                {
                   string result =  await _manager.EnableDisableUser(userId,roleName);
                    response.StatusCode = 200;
                    response.Message = $"User {result} Successfully!!";
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


        [HttpPatch("verify-employer-by-admin")]
        public async Task<IActionResult> VerifyEmployerByAdminAsync(int userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }

            var response = new BaseResponseAdd();
            try
            {
                if (userId > 0)
                {
                    await _manager.VerifyEmployerByAdmin(userId);
                    response.StatusCode = 200;
                    response.Message = $"Employer Verified Successfully!!";
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

    }
}