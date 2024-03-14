using FHP.infrastructure.DataLayer;
using FHP.infrastructure.Manager.FHP;
using FHP.infrastructure.Service;
using FHP.models.FHP.EmployeeAvailability;
using FHP.utilities;
using Microsoft.AspNetCore.Mvc;

namespace FHP.Controllers.FHP
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeAvailabilityController : ControllerBase
    {
        private readonly IEmployeeAvailabilityManager _manager;
        private readonly IExceptionHandleService _exceptionHandleService;
        private readonly IUnitOfWork _unitOfWork;


        public EmployeeAvailabilityController(IEmployeeAvailabilityManager manager,
             IExceptionHandleService exceptionHandleService,
             IUnitOfWork unitOfWork)
        {
            _manager=manager;
            _exceptionHandleService=exceptionHandleService;
            _unitOfWork=unitOfWork;
        }

        [HttpPost("add")] //Add EmployeeAvailability 
        public async Task<IActionResult> AddAsync(AddEmployeeAvailabilityModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList()); //it returns a BadRequest response with a list of errors.
            }

            var response = new BaseResponseAdd();

            await using var  transaction = await _unitOfWork.BeginTransactionAsync(); //The method then begins a database transaction to ensure data consistency during  addition.

            try
            {
                if(model.Id == 0 && model.UserId != 0 && model.JobId != 0 && model.EmployeeId != 0)
                {
                    // Add the EmployeeAvailability model asynchronously.
                    await _manager.AddAsync(model);
                   
                    // Commit the transaction.
                    await transaction.CommitAsync(); 
                    response.StatusCode = 200;
                    response.Message = Constants.added;
                    return Ok(response);
                }

                // If necessary fields are not provided in the model, return a BadRequest response.
                response.StatusCode = 400;
                response.Message = Constants.provideValues;
                return BadRequest(response);

            }
            catch(Exception ex)
            {
                await transaction.RollbackAsync(); //In case of any exceptions during the process, it rolls back the transaction.
                return await _exceptionHandleService.HandleException(ex); //exceptionHandle service
            }
        }


        [HttpPut("edit")] //Edit EmployeeAvailability
        public async Task<IActionResult> EditAsync(AddEmployeeAvailabilityModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList()); //it returns a BadRequest response with a list of errors.
            }

            var response = new BaseResponseAdd();
            await using var transaction = await _unitOfWork.BeginTransactionAsync(); //The method then begins a database transaction to ensure data consistency during  updation.

            try
            {
                if(model.Id >= 0)
                {
                    // Edit the EmployeeAvailability model asynchronously.
                    await _manager.Edit(model);

                    // commit transaction
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
                await transaction.RollbackAsync();  //In case of any exceptions during the process, it rolls back the transaction.

                return await _exceptionHandleService.HandleException(ex);    

            }
        }

        [HttpGet("getall-pagination")] // GetAll EmployeeAvalibility Detail with pagination and search filter
        public async Task<IActionResult> GetAllAsync(int page , int pageSize,string? search)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList()); //it returns a BadRequest response with a list of errors.
            }

             var response = new BaseResponsePagination<object>(); 
            try
            {

                // Retrieve data from the manager based on pagination parameters.
                var data = await _manager.GetAllAsync(page,pageSize,search);

                // Check if data is retrieved successfully.
                if (data.employeeAval != null)
                {
                    response.StatusCode = 200;
                    response.Data = data.employeeAval;
                    response.TotalCount = data.totalCount;
                    return Ok(response);
                }

                response.StatusCode = 400;
                response.Message = Constants.error;
                return BadRequest(response);
            }
            catch(Exception ex)
            {
                return await _exceptionHandleService.HandleException(ex);  //exceptionHandle service
            }
        }


        [HttpGet("GetByEmployeeId")] //GetByEmployeeId 
        public async Task<IActionResult> GetByEmployeeIdAsync(int employeeId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList()); //it returns a BadRequest response with a list of errors.
            }

            var response = new BaseResponseAddResponse<object>();

            try
            {
                // Retrieve AdminSelectEmployee data by Employee Id from the manager.
                var data = await _manager.GetByEmployeeIdAsync(employeeId);

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
            catch(Exception ex)
            {
                return await _exceptionHandleService.HandleException(ex); // error handling service
            }
        }


        [HttpGet("getbyid")]  //GetById EmployeeAvailability
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList()); //it returns a BadRequest response with a list of errors.
            }

            var response = new BaseResponseAddResponse<object>();
            try
            {

                // Retrieve EmployeeAvailability data by its Id from the manager.
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
                return await _exceptionHandleService.HandleException(ex);  // exceptionhandler service
            }
        }


        [HttpGet("SetEmployeeAvalibility")] // Employee can set his/her availibility for the job
        public async Task<IActionResult> SetEmployeeAvalibiliyAsync(int EmployeeId, int JobId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList()); //it returns a BadRequest response with a list of errors.
            }

            var response = new BaseResponseAdd();

            try
            {
                if(EmployeeId <= 0)
                {
                    // If EmployeeId is not provided or invalid, return a BadRequest response.
                    response.StatusCode = 400;
                    response.Message = "Id Required!";
                    return BadRequest(response);
                }
                // Call the manager method to set Employee availability for the job.
                string result = await _manager.SetEmployeeAvalibility(EmployeeId,JobId);
                response.StatusCode = 200;
                response.Message = $"Employee {result} Now!!"; // SetEmployeeAvalibiliy Succesfully!
                return Ok(response);
            }
            catch(Exception ex)
            {
               return await _exceptionHandleService.HandleException(ex); // exceptionhandler service
            }
        }
       
        [HttpGet("GetAllAvalibility")] //GetAll by EmployeeAvalibility
        public async Task<IActionResult> GetAllAvalibility(int JobId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList()); //it returns a BadRequest response with a list of errors.
            }

            var response = new BaseResponseAddResponse<object>();

            try
            {

                // Call the manager method to get Employee availability by job id for the job.
                var data = await _manager.GetAllAvalibility(JobId); 
                if (data != null)
                 {
                    response.StatusCode = 200;
                    response.Data=data;
                    return Ok(response);
                 }

                // If data retrieval fails, return a BadRequest response.
                response.StatusCode = 400;
                response.Message = Constants.error;
                return BadRequest(response);

            }

            catch( Exception ex)
            {
                return await _exceptionHandleService.HandleException(ex);  // exceptionhandler service
            }
        }

        [HttpDelete("delete/{id}")] //delete EmployeeAvalibility
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList()); //it returns a BadRequest response with a list of errors.
            }

            var response = new BaseResponseAdd();

            try
            {
                if (id <= 0)
                {

                    // If Id is not provided or invalid, return a BadRequest response.
                    response.StatusCode = 400;
                    response.Message = " Id Required ";
                    return BadRequest(response);
                }

                // Delete EmployeeAvailability asynchronously using the manager.
                await _manager.DeleteAsync(id);
                response.StatusCode = 200;
                response.Message = Constants.deleted;
                return Ok(response);

            }
            catch (Exception ex)
            {
                return await _exceptionHandleService.HandleException(ex); // exceptionhandler service
            }
        }
    }
}
