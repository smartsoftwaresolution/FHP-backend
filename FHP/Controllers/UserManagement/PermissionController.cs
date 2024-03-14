using FHP.entity.UserManagement;
using FHP.infrastructure.Manager.UserManagement;
using FHP.infrastructure.Service;
using FHP.models.UserManagement.Permission;
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

  
        [HttpPost("add")] //  API Endpoint for adding a new permission
        public async Task<IActionResult> AddAsync(AddPermissionModel model)
        {
            if (!ModelState.IsValid)
            {
                // Returns a BadRequest response with a list of errors if model state is not valid
                return BadRequest(ModelState.GetErrorList());  
            }

            // Initializes the response object for returning the result
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

                    // Returns Ok response with the success message
                    return Ok(response);
                }

                response.StatusCode = 400;
                response.Message = Constants.provideValues;

                // Returns BadRequest response with the error message
                return BadRequest(response);

            }
            catch(Exception ex)
            {
                // Handle the exception using the provided exception handling service.
                return await _exceptionHandleService.HandleException(ex); 
            }
        }


        
        [HttpPut("edit")] // API Endpoint for editing a permission
        public async Task<IActionResult> EditAsync(AddPermissionModel model)
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
                // Checks if the model ID is greater than or equal to 0
                if (model.Id>=0 && model != null)
                {
                    await _manager.EditAsync(model);

                    // Sets StatusCode to 200 indicating success
                    response.StatusCode = 200;
                    response.Message = Constants.updated;

                    // Returns Ok response with the success message
                    return Ok(response);
                }

                response.StatusCode = 400;
                response.Message = Constants.provideValues;

                // Returns BadRequest response with the error message
                return BadRequest(response);

            }
            catch(Exception ex)
            {
                // Handle the exception using the provided exception handling service.
                return await _exceptionHandleService.HandleException(ex);
            }
        }

        
        [HttpGet("getall-pagination")] // API Endpoint for retrieving all permission with pagination
        public async Task<IActionResult> GetAllAsync(int page,int pageSize,string? search)
        {
            // Checks if the model state is valid
            if (!ModelState.IsValid)
            {
                // Returns a BadRequest response with a list of errors if model state is not valid
                return BadRequest(ModelState.GetErrorList()); 
            }

            // Initializes the response object for returning the result
            var response = new BaseResponsePagination<object>();

            try
            {
                // Calls the manager to retrieve permissions asynchronously with pagination and search
                var data = await _manager.GetAllAsync(page,pageSize,search);

                // Checks if the retrieved data is not null
                if (data.permission != null)
                {
                    // Sets StatusCode to 200 indicating success
                    response.StatusCode = 200;
                    response.Data = data.permission;
                    response.TotalCount = data.totalCount;

                    // Returns Ok response with the data
                    return Ok(response);
                }

                response.StatusCode = 400;
                response.Message= Constants.error;

                // Returns BadRequest response with the error message
                return BadRequest(response);    

            }
            catch(Exception ex)
            {
                // Handle the exception using the provided exception handling service.
                return await _exceptionHandleService.HandleException(ex); 
                    
            }
        }

       
       
        [HttpGet("getbyid")] // API Endpoint for retrieving a permission by its ID
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            // Checks if the model state is valid
            if (!ModelState.IsValid)
            {
                // Returns a BadRequest response with a list of errors if model state is not valid
                return BadRequest(ModelState.GetErrorList());   
            }

            // Initializes the response object for returning the result
            var response = new BaseResponseAddResponse<object>();

            try
            {
                // Calls the manager to retrieve a permission by its ID asynchronously
                var data = await _manager.GetByIdAsync(id);

                // Checks if the retrieved data is not null
                if (data != null)
                {
                    // Sets StatusCode to 200 indicating success
                    response.StatusCode = 200;
                    response.Data = data;

                    // Returns Ok response with the data
                    return Ok(response);

                }

                response.StatusCode = 400;
                response.Message= Constants.error;

                // Returns BadRequest response with the error message
                return BadRequest(response);
            }
            catch(Exception ex)
            {
                // Handle the exception using the provided exception handling service.
                return await _exceptionHandleService.HandleException(ex);  
            }
        }

      
        [HttpDelete("delete/{id}")] // API Endpoint for deleting a permission by its ID
        public async Task<IActionResult> DeleteAsync(int id)
        {
            // Checks if the model state is valid
            if (!ModelState.IsValid)
            {
                // Returns a BadRequest response with a list of errors if model state is not valid
                return BadRequest(ModelState.GetErrorList()); 
            }

            var response = new BaseResponseAdd();

            try
            {
                // Checks if the provided ID is less than or equal to 0
                if (id <= 0)
                {
                    response.StatusCode = 400;
                    response.Message = "Id required.";

                    // Returns BadRequest response with the error message
                    return BadRequest(response);
                }

                // Calls the manager to delete a permission by its ID asynchronously
                await _manager.DeleteAsync(id);

                // Sets StatusCode to 200 indicating success
                response.StatusCode = 200;
                response.Message = Constants.deleted;

                // Returns Ok response with the success message
                return Ok(response);
            }
            catch(Exception ex)
            {
                // Handle the exception using the provided exception handling service.
                return await _exceptionHandleService.HandleException(ex); 
            }
        }
    }
}
