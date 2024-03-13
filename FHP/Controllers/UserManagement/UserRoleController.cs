using FHP.entity.UserManagement;
using FHP.infrastructure.DataLayer;
using FHP.infrastructure.Manager.UserManagement;
using FHP.infrastructure.Service;
using FHP.models.UserManagement.UserRole;
using FHP.utilities;
using Microsoft.AspNetCore.Mvc;

namespace FHP.Controllers.UserManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {
        private readonly IUserRoleManager _manager;
        private readonly IExceptionHandleService _exceptionHandleService;
        private readonly IUnitOfWork _unitOfWork;
        public UserRoleController(IUserRoleManager manager,
                                  IExceptionHandleService exceptionHandleService,
                                  IUnitOfWork unitOfWork)
        {
            _manager = manager;
            _exceptionHandleService = exceptionHandleService;
            _unitOfWork = unitOfWork;
        }

        
        [HttpPost("add")] // API Endpoint for adding a user role
        public async Task<IActionResult> AddAsync(AddUserRoleModel model)
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

                // Checks if the model ID is 0 and roleName is not empty or null
                if (model.Id == 0  && 
                    !string.IsNullOrEmpty(model.RoleName))
                {

                    // Calls the manager to add the user role asynchronously
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
            catch (Exception ex)
            {
                //In case of any exceptions during the process, it rolls back the transaction
                await transaction.RollbackAsync();

                // Handle the exception using the provided exception handling service.
                return await _exceptionHandleService.HandleException(ex);

            }
        }

        
        [HttpPut("edit")] //  API Endpoint for editing a user role
        public async Task<IActionResult> Edit(AddUserRoleModel model)
        {
            // Checks if the model state is valid
            if (!ModelState.IsValid)
            {
                // Returns a BadRequest response with a list of errors if model state is not valid
                return BadRequest(ModelState.GetErrorList()); 
            }

            //The method then begins a database transaction to ensure data consistency during  updation.
            await using var transaction = await _unitOfWork.BeginTransactionAsync();

            // Initializes the response object for returning the result
            var response = new BaseResponseAdd();

            try
            {
                // Checks if the model ID is greater than or equal to 0
                if (model.Id >= 0 )
                {
                    // Calls the manager to edit the user role asynchronously
                    await _manager.EditAsync(model); 
                    await transaction.CommitAsync(); 

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
            catch (Exception ex)
            {
                //In case of any exceptions during the process, it rolls back the transaction.
                await transaction.RollbackAsync();

                // Handle the exception using the provided exception handling service.
                return await _exceptionHandleService.HandleException(ex); 
            }
        }
       
        
        [HttpGet("getall-pagination")] // API Endpoint for retrieving all user roles with pagination
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
                // Calls the manager to retrieve user roles asynchronously with pagination and search
                var data = await _manager.GetAllAsync(page,pageSize,search);

                // Checks if the retrieved data is not null
                if (data.userRole != null)
                {
                    // Sets StatusCode to 200 indicating success
                    response.StatusCode = 200;
                    response.Data = data.userRole;
                    response.TotalCount = data.totalCount;

                    // Returns Ok response with the data
                    return Ok(response);
                }

                response.StatusCode = 400;
                response.Message = Constants.error;

                // Returns BadRequest response with the error message
                return BadRequest(response);
            }
            catch (Exception ex)
            {

                // Handle the exception using the provided exception handling service.
                return await _exceptionHandleService.HandleException(ex); 
            }
        }

        
        [HttpGet("getbyid")] // API Endpoint for retrieving a user role by its ID
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
                // Calls the manager to retrieve a user role by its ID asynchronously
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
                response.Message = Constants.error;

                // Returns BadRequest response with the error message
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                // Handle the exception using the provided exception handling service.
                return await _exceptionHandleService.HandleException(ex); 

            }
        }


       
        [HttpDelete("delete/{id}")] // API Endpoint for deleting a user role by its ID
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
                    response.Message = "Id Required";
                    response.StatusCode = 400;

                    // Returns BadRequest response with the error message
                    return BadRequest(response);
                }

                // Calls the manager to delete a user role by its ID asynchronously
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
