using FHP.infrastructure.DataLayer;
using FHP.infrastructure.Manager.FHP;
using FHP.infrastructure.Manager.UserManagement;
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
        private readonly ISendNotificationService _sendNotificationService;
        private readonly IFCMTokenManager _tokenManager;

        public EmployeeAvailabilityController(IEmployeeAvailabilityManager manager,
             IExceptionHandleService exceptionHandleService,
             IUnitOfWork unitOfWork,
             ISendNotificationService sendNotificationService,
             IFCMTokenManager tokenManager)
        {
            _manager=manager;
            _exceptionHandleService=exceptionHandleService;
            _unitOfWork=unitOfWork;
            _sendNotificationService = sendNotificationService;
            _tokenManager = tokenManager;
        }

        //Add EmployeeAvailability
        [HttpPost("add")]  
        public async Task<IActionResult> AddAsync(AddEmployeeAvailabilityModel model)
        {
            if (!ModelState.IsValid)
            {
                //it returns a BadRequest response with a list of errors.
                return BadRequest(ModelState.GetErrorList()); 
            }

            var response = new BaseResponseAdd();

            //The method then begins a database transaction to ensure data consistency during  addition.
            await using var  transaction = await _unitOfWork.BeginTransactionAsync(); 

            try
            {
                if(model.Id == 0 && model.UserId != 0 && model.JobId != 0 && model.EmployeeId != null)
                {
                    // Add the EmployeeAvailability model asynchronously.
                    await _manager.AddAsync(model);

                    /*var token = await _tokenManager.FcmTokenByRole("employee");

                    foreach(var t in token)
                    {
                        string body = $"Hello Dear \n\nWe hope this message finds you well. We are writing to inquire about your availability regarding the job opportunity recently posted for [Job Position]. Could you please let us know your current availability and any potential constraints regarding the job?\n\nYour prompt response would be highly appreciated.\n\nBest regards";
                        await _sendNotificationService.SendNotification("Job Availability Inquiry", body, t.TokenFCM);
                    }*/
                   
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
                //In case of any exceptions during the process, it rolls back the transaction.
                await transaction.RollbackAsync(); 
                return await _exceptionHandleService.HandleException(ex); 

            }
        }

        //Edit EmployeeAvailability
        [HttpPut("edit")] 
        public async Task<IActionResult> EditAsync(AddEmployeeAvailabilityModel model)
        {
            if (!ModelState.IsValid)
            {
                //it returns a BadRequest response with a list of errors.
                return BadRequest(ModelState.GetErrorList()); 
            }

            var response = new BaseResponseAdd();

            //The method then begins a database transaction to ensure data consistency during  updation.
            await using var transaction = await _unitOfWork.BeginTransactionAsync(); 

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
                //In case of any exceptions during the process, it rolls back the transaction.
                await transaction.RollbackAsync(); 

                return await _exceptionHandleService.HandleException(ex);    

            }
        }

        // GetAll EmployeeAvalibility Detail with pagination and search filter
        [HttpGet("getall-pagination")] 
        public async Task<IActionResult> GetAllAsync(int page , int pageSize,string? search,int employeeId, Constants.EmployeeAvailability? employeeAvailability)
        {
            if (!ModelState.IsValid)
            {
                //it returns a BadRequest response with a list of errors.
                return BadRequest(ModelState.GetErrorList()); 
            }

             var response = new BaseResponsePagination<object>(); 
             
            try
            {

                // Retrieve data from the manager based on pagination parameters.
                var data = await _manager.GetAllAsync(page,pageSize,search,employeeId,employeeAvailability);

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
                //exceptionHandle service
                return await _exceptionHandleService.HandleException(ex); 
            }
        }

        //GetByEmployeeId 
        [HttpGet("GetByEmployeeId")] 
        public async Task<IActionResult> GetByEmployeeIdAsync(int employeeId,string? search, Constants.EmployeeAvailability? IsAvailable)
        {
            if (!ModelState.IsValid)
            {
                //it returns a BadRequest response with a list of errors.
                return BadRequest(ModelState.GetErrorList()); 
            }

            var response = new BaseResponseAddResponse<object>();

            try
            {
                // Retrieve AdminSelectEmployee data by Employee Id from the manager.
                var data = await _manager.GetByEmployeeIdAsync(employeeId,search, IsAvailable);

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
                // error handling service
                return await _exceptionHandleService.HandleException(ex); 
            }
        }

        //GetById EmployeeAvailability
        [HttpGet("getbyid")]  
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                //it returns a BadRequest response with a list of errors.
                return BadRequest(ModelState.GetErrorList()); 
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
                // exceptionhandler service
                return await _exceptionHandleService.HandleException(ex);  
            }
        }


        // Employee can set his/her availibility for the job
        [HttpPost("SetEmployeeAvalibility")] 
        public async Task<IActionResult> SetEmployeeAvalibiliyAsync(SetEmployeeAvailabilityModel model)
        {
            if (!ModelState.IsValid)
            {
                //it returns a BadRequest response with a list of errors.
                return BadRequest(ModelState.GetErrorList()); 
            }

            var response = new BaseResponseAdd();

            try
            {
                if(model.EmployeeId <= 0)
                {
                    // If EmployeeId is not provided or invalid, return a BadRequest response.
                    response.StatusCode = 400;
                    response.Message = "Id Required!";
                    return BadRequest(response);
                }
                
              

                // Call the manager method to set Employee availability for the job.
                string result = await _manager.SetEmployeeAvalibility(model);

                if (model.EmployeeAvailability == Constants.EmployeeAvailability.Available)
                {
                    var token = await _tokenManager.FcmTokenByRole("admin");

                    foreach (var t in token)
                    {
                        string body = "An employee is available for Job.";
                        await _sendNotificationService.SendNotification("Employee Avalibililty", body, t.TokenFCM);
                    }
                }


                response.StatusCode = 200;
                response.Message = $"Employee {result} Now!!"; 
                return Ok(response);
            }
            catch(Exception ex)
            {
                // exceptionhandler service
                return await _exceptionHandleService.HandleException(ex); 
            }
        }

        //GetAll by EmployeeAvalibility
        [HttpGet("GetAllAvalibility")] 
        public async Task<IActionResult> GetAllAvalibility(int page,int pageSize, string? search,int JobId,Constants.EmployeeAvailability? employeeAvailability)
        {
            if (!ModelState.IsValid)
            {
                //it returns a BadRequest response with a list of errors.
                return BadRequest(ModelState.GetErrorList()); 
            }

            var response = new BaseResponsePagination<object>();

            try
            {

                // Call the manager method to get Employee availability by job id for the job.
                var data = await _manager.GetAllAvalibility(page,pageSize, search,JobId,employeeAvailability); 
                
                if (data.getallAval != null  && data.totalCount > 0)
                {
                    response.StatusCode = 200;
                    response.Data = data.getallAval;
                    response.TotalCount = data.totalCount;
                    return Ok(response);
                }

                // If data retrieval fails, return a BadRequest response.
                response.StatusCode = 400;
                response.Message = Constants.error;
                return BadRequest(response);

            }

            catch( Exception ex)
            {
                // exceptionhandler service
                return await _exceptionHandleService.HandleException(ex);  
            }
        }

        //delete EmployeeAvalibility
        [HttpDelete("delete/{id}")] 
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                //it returns a BadRequest response with a list of errors.
                return BadRequest(ModelState.GetErrorList()); 
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
                // exceptionhandler service
                return await _exceptionHandleService.HandleException(ex); 
            }
        }



        /*[HttpGet("GetByJobId")] 
        public async Task<IActionResult> GetByJobIdAsync(int jobId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList()); //it returns a BadRequest response with a list of errors.
            }

            var response = new BaseResponseAddResponse<object>();

            try
            {
                // Retrieve AdminSelectEmployee data by Employee Id from the manager.
                var data = await _manager.GetByJobIdAsync(jobId);

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
                return await _exceptionHandleService.HandleException(ex); // error handling service
            }
        }
*/
    }
}
