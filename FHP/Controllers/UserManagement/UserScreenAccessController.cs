using Castle.Core.Logging;
using DocumentFormat.OpenXml.ExtendedProperties;
using FHP.infrastructure.Manager.UserManagement;
using FHP.infrastructure.Service;
using FHP.models.UserManagement;
using FHP.services;
using FHP.utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FHP.Controllers.UserManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserScreenAccessController : ControllerBase
    {
        private readonly IUserScreenAccessManager _manager;
        private readonly IExceptionHandleService _exceptionHandleService;

        public UserScreenAccessController(IUserScreenAccessManager manager,IExceptionHandleService exceptionHandleService)
        {
            _manager= manager;
            _exceptionHandleService= exceptionHandleService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddAsync(AddUserScreenAccessModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }

            var response = new BaseResponseAdd();

            try
            {
                if(model.Id == 0 && model.RoleId !=0 && model.ScreenId !=0)
                {
                    await _manager.AddAsync(model);
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
                return await _exceptionHandleService.HandleException(ex);
            }
        }


        [HttpPut("edit")]
        public async Task<IActionResult> EditAsync(AddUserScreenAccessModel model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }
            var response = new BaseResponseAdd();

            try
            {
                if(model.Id >=0 && model != null)
                {
                    await _manager.Edit(model);
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
                return await _exceptionHandleService.HandleException(ex);
            }
        }


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
