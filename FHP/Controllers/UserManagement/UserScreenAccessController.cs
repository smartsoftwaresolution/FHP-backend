using Castle.Core.Logging;
using DocumentFormat.OpenXml.ExtendedProperties;
using FHP.infrastructure.DataLayer;
using FHP.infrastructure.Manager.UserManagement;
using FHP.infrastructure.Service;
using FHP.models.UserManagement.UserScreenAccess;
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

      
        [HttpPost("add")] // API Endpoint for adding a user screen access
        public async Task<IActionResult> AddAsync(AddUserScreenAccessModel model)
        {
            // Checks if the model state is valid
            if (!ModelState.IsValid)
            {
                // Returns a BadRequest response with a list of errors if model state is not valid
                return BadRequest(ModelState.GetErrorList()); 
            }

            // Initializes the response object for returning the result
            var response = new BaseResponseAdd();

            //The method then begins a database transaction to ensure data consistency during  addition.
            await using var transaction = await _unitOfWork.BeginTransactionAsync(); 

            try
            {
                if(model.Id == 0 && model.RoleId !=0 && model.ScreenId !=0)
                {
                    await _manager.AddAsync(model);

                    //commit the transaction
                    await transaction.CommitAsync();

                    // Sets StatusCode to 200 indicating success
                    response.StatusCode = 200;
                    response.Message = Constants.added;

                    // Returns Ok response with the success message
                    return Ok(response);
                }

                // Sets StatusCode to 400 indicating a bad request
                response.StatusCode = 400;
                response.Message = Constants.provideValues;

                // Returns BadRequest response with the error message
                return BadRequest(response);
            }
            catch(Exception ex)
            {

                //In case of any exceptions during the process, it rolls back the transaction
                await transaction.RollbackAsync();

                // Handle the exception using the provided exception handling service.
                return await _exceptionHandleService.HandleException(ex); 
            }
        }


       
        [HttpPut("edit")] // API Endpoint for updating existing  user screen access
        public async Task<IActionResult> EditAsync(AddUserScreenAccessModel model)
        {
            // Checks if the model state is valid
            if (!ModelState.IsValid)
            {
                // Returns a BadRequest response with a list of errors if model state is not valid
                return BadRequest(ModelState.GetErrorList()); 
            }

            // Initializes the response object for returning the result
            var response = new BaseResponseAdd();

            //The method then begins a database transaction to ensure data consistency during  updation.
            await using var transaction = await _unitOfWork.BeginTransactionAsync();  

            try
            {
                if(model.Id >=0 && model != null)
                {
                    await _manager.Edit(model);

                    //commit the transaction
                    await transaction.CommitAsync();

                    // Sets StatusCode to 200 indicating success
                    response.StatusCode = 200;
                    response.Message = Constants.updated;

                    // Returns Ok response with the success message
                    return Ok(response);
                }


                // Sets StatusCode to 400 indicating a bad request
                response.StatusCode = 400;
                response.Message = Constants.provideValues;

                // Returns BadRequest response with the error message
                return BadRequest(response);
            }
            catch(Exception ex)
            {
                //In case of any exceptions during the process, it rolls back the transaction
                await transaction.RollbackAsync();  

                // Handle the exception using the provided exception handling service.
                return await _exceptionHandleService.HandleException(ex);
            }
        }

        
        [HttpGet("getall-pagination")] // API Endpoint for retrieving all user roles with pagination
        public async Task<IActionResult> GetAllAsync(int roleId,int page,int pageSize)
        {
            // Checks if the model state is valid
            if (!ModelState.IsValid)
            {
                // Returns a BadRequest response with a list of errors if model state is not valid
                return BadRequest(ModelState.GetErrorList());  
            }

            // Initializes the response object for returning the result
            var response =new BaseResponsePagination<object>();

            try
            {
                // Calls the manager to retrieve all user screen access with pagination based on role ID asynchronously
                var data = await _manager.GetAllAsync(roleId,page,pageSize);

                // Checks if the retrieved data is not null
                if (data.userScreenAccess != null)
                {
                    // Sets StatusCode to 200 indicating success
                    response.StatusCode = 200;
                    response.Data = data.userScreenAccess;
                    response.TotalCount = data.totalCount;

                    // Returns Ok response with the data
                    return Ok(response);
                }

                // Sets StatusCode to 400 indicating a bad request
                response.StatusCode = 400;
                response.Message = Constants.error;

                // Returns BadRequest response with the error message
                return BadRequest(response);
            }
            catch(Exception ex)
            {
                // Handle the exception using the provided exception handling service.
                return await _exceptionHandleService.HandleException(ex); 
            }
        }

        
        [HttpGet("getbyid")] //  API Endpoint for retrieving a user role by its ID
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
                // Calls the manager to retrieve  by its ID asynchronously
                var data = await _manager.GetByIdAsync(id);

                // Checks if the retrieved data is not null
                if (data != null)
                {

                    // Checks if the retrieved data is not null
                    response.StatusCode = 200;
                    response.Data = data;

                    // Returns Ok response with the data
                    return Ok(response);
                }

                response.StatusCode = 400;
                response.Message = Constants.error;

                // Returns BadRequest response with the error message
                return BadRequest(response);
            }
            catch(Exception ex)
            {
                // Handle the exception using the provided exception handling service.
                return await _exceptionHandleService.HandleException(ex); 
            }
        }


       
        [HttpDelete("delete/{id}")] //  API Endpoint for deleting an entity by its ID
        public async Task<IActionResult> DeleteAsync(int id)
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
                // Checks if the provided ID is less than or equal to 0
                if (id <= 0)
                {
                    // Sets StatusCode to 400 indicating a bad request
                    response.StatusCode = 400;
                    response.Message = "Id required.";

                    // Returns BadRequest response with the error message
                    return BadRequest(response);
                }

                // Calls the manager to delete an entity by its ID asynchronously
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
