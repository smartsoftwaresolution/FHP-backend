using FHP.infrastructure.Manager.UserManagement;
using FHP.infrastructure.Service;
using FHP.models.UserManagement;
using FHP.services;
using FHP.utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Bcpg.OpenPgp;

namespace FHP.Controllers.UserManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionManager _manager;
        private readonly IExceptionHandleService _exceptionHandleService;

        public PermissionController(IPermissionManager manager,IExceptionHandleService exceptionHandleService)
        {
            _manager = manager;
            _exceptionHandleService= exceptionHandleService;
        }

        // add permission
        [HttpPost("add")]
        public async Task<IActionResult> AddAsync(AddPermissionModel model)
        {
            if (!ModelState.IsValid)
            {
              return BadRequest(ModelState.GetErrorList());   
            }

            var response = new BaseResponseAdd();

            try
            {

                if(model.Id ==0 && model.ScreenId !=0 &&
                    !string.IsNullOrEmpty(model.Permissions) &&
                    !string.IsNullOrEmpty(model.PermissionDescription) &&
                    !string.IsNullOrEmpty(model.PermissionCode) &&
                    !string.IsNullOrEmpty(model.ScreenCode) &&
                    !string.IsNullOrEmpty(model.ScreenUrl))
                {
                    await _manager.AddAsync(model); // added
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


        // edit permission
        [HttpPut("edit")]
        public async Task<IActionResult> EditAsync(AddPermissionModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }

            var response = new BaseResponseAdd();

            try
            {
     
                if(model.Id>=0 && model != null)
                {
                    await _manager.EditAsync(model); //updated
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

        // get all permission
        [HttpGet("getall-pagination")]
        public async Task<IActionResult> GetAllAsync(int page,int pageSize,string? search)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }

            var response = new BaseResponsePagination<object>();

            try
            {
                var data = await _manager.GetAllAsync(page,pageSize,search);
                if(data.permission != null)
                {
                    response.StatusCode = 200;
                    response.Data = data.permission;
                    response.TotalCount = data.totalCount;
                    return Ok(response);
                }

                response.StatusCode = 400;
                response.Message= Constants.error;
                return BadRequest(response);    

            }
            catch(Exception ex)
            {
                return await _exceptionHandleService.HandleException(ex);
                    
            }
        }

       
        // get by id Permission 
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
                response.Message= Constants.error;
                return BadRequest(response);
            }
            catch(Exception ex)
            {
                return await _exceptionHandleService.HandleException(ex);
            }
        }

        // delete permission
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
              
                if(id <= 0)
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
