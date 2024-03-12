using Castle.Core.Logging;
using DocumentFormat.OpenXml.ExtendedProperties;
using FHP.infrastructure.DataLayer;
using FHP.infrastructure.Manager.UserManagement;
using FHP.infrastructure.Service;
using FHP.models.UserManagement;
using FHP.services;
using FHP.utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NAudio.Midi;

namespace FHP.Controllers.UserManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserScreenAccessController : ControllerBase
    {
        private readonly IUserScreenAccessManager _manager;
        private readonly IExceptionHandleService _exceptionHandleService;
        private readonly IUnitOfWork _unitOfWork;
        public UserScreenAccessController(IUserScreenAccessManager manager,
                                          IExceptionHandleService exceptionHandleService,
                                          IUnitOfWork unitOfWork)
        {
            _manager= manager;
            _exceptionHandleService= exceptionHandleService;
            _unitOfWork= unitOfWork;
        }

        // add UserScreenAccess
        [HttpPost("add")]
        public async Task<IActionResult> AddAsync(AddUserScreenAccessModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }

            var response = new BaseResponseAdd();
            await using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                if(model.Id == 0 && model.RoleId !=0 && model.ScreenId !=0)
                {
                    await _manager.AddAsync(model); // added
                    await transaction.CommitAsync();
                    response.StatusCode = 200;
                    response.Message = Constants.added;
                    return Ok(response);
                }

                response.StatusCode = 400;
                response.Message = Constants.provideValues;
                return BadRequest(response);
            }
            catch(Exception ex)
            {
                await transaction.RollbackAsync();
                return await _exceptionHandleService.HandleException(ex);
            }
        }


        // edit UserScreenAccess
        [HttpPut("edit")]
        public async Task<IActionResult> EditAsync(AddUserScreenAccessModel model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }
            var response = new BaseResponseAdd();
            await using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                if(model.Id >=0 && model != null)
                {
                    await _manager.Edit(model); //updated
                    await transaction.CommitAsync();
                    response.StatusCode = 200;
                    response.Message = Constants.updated;
                    return Ok(response);
                }

                response.StatusCode = 400;
                response.Message = Constants.provideValues;
                return BadRequest(response);
            }
            catch(Exception ex)
            {
                await transaction.RollbackAsync();
                return await _exceptionHandleService.HandleException(ex);
            }
        }

        //get all userRolesScreen
        [HttpGet("getall-pagination")]
        public async Task<IActionResult> GetAllAsync(int roleId,int page,int pageSize)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }

            var response =new BaseResponsePagination<object>();

            try
            {
                var data = await _manager.GetAllAsync(roleId,page,pageSize);
                if(data.userScreenAccess != null)
                {
                    response.StatusCode = 200;
                    response.Data = data.userScreenAccess;
                    response.TotalCount = data.totalCount;
                    return Ok(response);
                }

                response.StatusCode = 400;
                response.Message = Constants.error;
                return BadRequest(response);
            }
            catch(Exception ex)
            {
                return await _exceptionHandleService.HandleException(ex);
            }
        }

        // get by id UserScreen Access
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
                if(data != null)
                {
                    response.StatusCode = 200;
                    response.Data = data;
                    return Ok(response);
                }

                response.StatusCode = 400;
                response.Message = Constants.error;
                return BadRequest(response);
            }
            catch(Exception ex)
            {
                return await _exceptionHandleService.HandleException(ex);
            }
        }


        // delete UserScreenAccess
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
                    response.Message = "Id required.";
                    return BadRequest(response);
                }
                await _manager.DeleteAsync(id);
                response.StatusCode = 200;
                response.Message = Constants.deleted;
                return Ok(response);
            }
            catch(Exception ex)
            {
                return await _exceptionHandleService.HandleException(ex);
            }
        }
    }
}
