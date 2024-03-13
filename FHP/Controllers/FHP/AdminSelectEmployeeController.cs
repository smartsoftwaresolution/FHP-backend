using FHP.infrastructure.DataLayer;
using FHP.infrastructure.Manager.FHP;
using FHP.infrastructure.Service;
using FHP.models.FHP.AdminSelectEmployee;
using FHP.utilities;
using Microsoft.AspNetCore.Mvc;

namespace FHP.Controllers.FHP
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminSelectEmployeeController : ControllerBase
    {
        private readonly IAdminSelectEmployeeManager _manager;
        private readonly IExceptionHandleService _exceptionHandleService;
        private readonly IUnitOfWork _unitOfWork;
        public AdminSelectEmployeeController(IAdminSelectEmployeeManager manager,
                                             IExceptionHandleService exceptionHandleService,
                                             IUnitOfWork unitOfWork)
        {
            _manager = manager;
            _exceptionHandleService= exceptionHandleService;
            _unitOfWork= unitOfWork;
        }


        [HttpPost("add")] // API endpoint to add AdminSelectEmployee 
        public async Task<IActionResult> AddAsync(AddAdminSelectEmployeeModel model)
        {
            if (!ModelState.IsValid)
            {
                // If the model state is not valid, return a BadRequest response with a list of errors.
                return BadRequest(ModelState.GetErrorList()); 
            }

            // Begin a database transaction to ensure data consistency during addition.
            await using var transaction = await _unitOfWork.BeginTransactionAsync();  
            
            
            var response = new BaseResponseAdd(); // Response object to be sent back.

            try
            {
                if(model.Id == 0 && model.JobId != 0 && model.EmployeeId != 0)
                {
                    // Add the AdminSelectEmployee model asynchronously.
                    await _manager.AddAsync(model);

                    // Commit the transaction.
                    await transaction.CommitAsync();

                    // Set response status code and message for successful addition.
                    response.StatusCode = 200;
                    response.Message = Constants.added;

                    // Return OK response with the success message.
                    return Ok(response);
                }

                // If necessary fields are not provided in the model, return a BadRequest response.
                response.StatusCode = 400;
                response.Message = Constants.provideValues;
                return BadRequest(response);    

            }
            catch(Exception ex)
            {
                // In case of any exceptions during the process, roll back the transaction.
                await transaction.RollbackAsync();

                // Handle the exception using the provided exception handling service.
                return await _exceptionHandleService.HandleException(ex);  
            }
        }

        [HttpPut("edit")] // API endpoint to edit AdminSelectEmployee 
        public async Task<IActionResult> EditAsync(AddAdminSelectEmployeeModel model)
        {
            if (!ModelState.IsValid)
            {
                // If the model state is not valid, return a BadRequest response with a list of errors.
                return BadRequest(ModelState.GetErrorList()); 
            }

            // Begin a database transaction to ensure data consistency during addition.
            await using var transaction = await _unitOfWork.BeginTransactionAsync();

            var response = new BaseResponseAdd(); // Response object to be sent back.

            try
            {
                if(model.Id >= 0)
                {
                    // Edit the AdminSelectEmployee model asynchronously.
                    await _manager.Edit(model);
                  
                    // Commit the transaction.
                    await transaction.CommitAsync();

                    // Set response status code and message for successful updation.
                    response.StatusCode = 200;
                    response.Message = Constants.updated;

                    // Return OK response with the success message.
                    return Ok(response);
                }

                // If necessary fields are not provided in the model, return a BadRequest response.
                response.StatusCode = 400;
                response.Message = Constants.provideValues;
                return BadRequest(response);

            }
            catch (Exception ex)
            {
                // In case of any exceptions during the process, roll back the transaction.
                await transaction.RollbackAsync();
               
                // Handle the exception using the provided exception handling service.
                return await _exceptionHandleService.HandleException(ex);    
            }
        }


        [HttpGet("getall-pagination")] // Get All AdminSelectEmpoyeeDetail with Pagination and search filter
        public async Task<IActionResult> GetAllAsync(int page,int pageSize,int jobId,string? search)
        {
            if (!ModelState.IsValid)
            {
                // If the model state is not valid, return a BadRequest response with a list of errors.
                return BadRequest(ModelState.GetErrorList()); 

            }

            var response = new BaseResponsePagination<object>(); // Response object for pagination.

            try
            {
                // Retrieve data from the manager based on pagination parameters.
                var data = await _manager.GetAllAsync(page,pageSize,jobId, search);

                // Check if data is retrieved successfully.
                if (data.adminSelect != null)
                {
                    // Set response status code, data, and total count for successful retrieval.
                    response.StatusCode = 200;
                    response.Data = data.adminSelect;
                    response.TotalCount = data.totalCount;

                    // Return OK response with the retrieved data.
                    return Ok(response);
                }
                // If data retrieval fails, return a BadRequest response.
                response.StatusCode = 400;
                response.Message = Constants.error;
                return BadRequest(response);

            }
            catch (Exception ex)
            {
                // Handle any exceptions using the provided exception handling service.
                return await _exceptionHandleService.HandleException(ex);  
            }
        }



        [HttpGet("getbyid")] //Get By Id AdminSelectEmployee
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            if (!ModelState.IsValid)
            { 
                return BadRequest(ModelState.GetErrorList());    //it returns a BadRequest response with a list of errors.
            }

            var response = new BaseResponseAddResponse<object>();

            try
            {
                // Retrieve AdminSelectEmployee data by its Id from the manager.
                var data = await _manager.GetByIdAsync(id);

                // Check if data is retrieved successfully.
                if (data != null)
                {
                    response.StatusCode = 200;
                    response.Data = data;
                    return Ok(response);
                }
                // If data retrieval fails, return a BadRequest response.
                response.StatusCode = 400;
                response.Message = Constants.error;
                return BadRequest(response);
            }

            catch (Exception ex)
            {
                return await _exceptionHandleService.HandleException(ex); //exception handle service
            }
              
        }

        [HttpDelete("delete/{id}")] //Delete AdminSelectEmployee by id
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (!ModelState.IsValid)
            { 
                return BadRequest(ModelState.GetErrorList());  //it returns a BadRequest response with a list of errors.
            }

            var response = new BaseResponseAdd();

            try
            {
                if(id <= 0)
                {
                    // If Id is not provided or invalid, return a BadRequest response.
                    response.StatusCode = 400;
                    response.Message = "Id Required.";
                    return BadRequest(response);
                }

                // Delete Contract asynchronously using the manager.
                await _manager.DeleteAsync(id);
                response.StatusCode=200;
                response.Message = Constants.deleted;
                return Ok(response);

            }
            catch(Exception ex)
            { 
                return await _exceptionHandleService.HandleException(ex);  //exception handle service
            }
        }


        [HttpGet("getall-job-employee")] //Get All JobEmployee Details
        public async Task<IActionResult> GetAllJobEmployeeAsync(int jobId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList()); //it returns a BadRequest response with a list of errors.
            }

            var response = new BaseResponsePagination<object>();

            try
            {
                var data = await _manager.GetAllJobEmployeeAsync(jobId);

                if (data.adminSelect != null)
                {
                    response.StatusCode = 200;
                    response.Data = data.adminSelect;
                    response.TotalCount = data.totalCount;
                    return Ok(response);
                }

                response.StatusCode = 400;
                response.Message = Constants.error;
                return BadRequest(response);

            }

            catch (Exception ex)
            { 
                return await _exceptionHandleService.HandleException(ex); //exception handle service
            }
        }

    }
}