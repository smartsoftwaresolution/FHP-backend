﻿
using FHP.infrastructure.DataLayer;
using FHP.infrastructure.Manager.FHP;
using FHP.infrastructure.Manager.UserManagement;
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
        private readonly ISendNotificationService _sendNotificationService;
        private readonly IFCMTokenManager _tokenManager;

        public AdminSelectEmployeeController(IAdminSelectEmployeeManager manager,
                                             IExceptionHandleService exceptionHandleService,
                                             IUnitOfWork unitOfWork,
                                             ISendNotificationService sendNotificationService,
                                             IFileUploadService fileUploadService,
                                             IFCMTokenManager tokenManager
                                             )
        {
            _manager = manager;
            _exceptionHandleService= exceptionHandleService;
            _unitOfWork= unitOfWork;
            _sendNotificationService = sendNotificationService;
            _tokenManager = tokenManager;
        }


        // API endpoint to add AdminSelectEmployee 
        [HttpPost("add")] 
        public async Task<IActionResult> AddAsync(AddAdminSelectEmployeeModel model)
        {
            if (!ModelState.IsValid)
            {
                // If the model state is not valid, return a BadRequest response with a list of errors.
                return BadRequest(ModelState.GetErrorList()); 
            }

            // Begin a database transaction to ensure data consistency during addition.
            await using var transaction = await _unitOfWork.BeginTransactionAsync();


            // Response object to be sent back.
            var response = new BaseResponseAdd(); 

            try
            {
                if(model.Id == 0 && model.JobId != 0 && model.EmployeeId != null)
                {
                    // Add the AdminSelectEmployee model asynchronously.
                    await _manager.AddAsync(model);


                     /* var token = await _tokenManager.FcmTokenByRole("employer");

                       foreach (var t in token)
                       {
                           if(t.UserId == model.EmployerId)
                           {
                               await _sendNotificationService.SendNotification("Sent", "OK", t.TokenFCM);
                           }
                       }*/

                    
                        var employertoken = await _tokenManager.FcmTokenByRole("employer");

                        var token = employertoken.FirstOrDefault();

                       if(token != null && token.UserId == model.EmployerId)
                       {
                         string message = "Ok";

                         await _sendNotificationService.SendNotification("Sent", message, token.TokenFCM);
                       }





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


        // API endpoint to edit AdminSelectEmployee 
        [HttpPut("edit")] 
        public async Task<IActionResult> EditAsync(AddAdminSelectEmployeeModel model)
        {
            if (!ModelState.IsValid)
            {
                // If the model state is not valid, return a BadRequest response with a list of errors.
                return BadRequest(ModelState.GetErrorList()); 
            }

            // Begin a database transaction to ensure data consistency during addition.
            await using var transaction = await _unitOfWork.BeginTransactionAsync();

            // Response object to be sent back.
            var response = new BaseResponseAdd(); 

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


        // Get All AdminSelectEmpoyeeDetail with Pagination and search filter
        [HttpGet("getall-pagination")] 
        public async Task<IActionResult> GetAllAsync(int page,int pageSize,int jobId,string? search,Constants.ProcessingStatus? status)
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
                var data = await _manager.GetAllAsync(page,pageSize,jobId, search,status);

                // Check if data is retrieved successfully.
                if (data.adminSelect != null )
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


        // API Endpoint for retrieving  by ID
        [HttpGet("getbyid")] 
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                // Returns a BadRequest response with a list of errors if model state is not valid
                return BadRequest(ModelState.GetErrorList()); 
            }

               // Initializes the response object for returning the result
               var response = new BaseResponseAddResponse<object>();

            try
            {
                // Retrieves employee data asynchronously by the provided ID
                var data = await _manager.GetByIdAsync(id);

                // Checks if data is found
                if (data != null)
                {
                    // Sets StatusCode to 200 indicating success
                    response.StatusCode = 200;

                    // Sets retrieved data to the response object
                    response.Data = data;

                    // Returns Ok response with the data
                    return Ok(response);
                }

                // If data retrieval fails, return a BadRequest response.
                response.StatusCode = 400;
                response.Message = "No data Found.";
                response.Data = "";
                return BadRequest(response);
            }

            catch (Exception ex)
            {
                // Handle any exceptions using the provided exception handling service.
                return await _exceptionHandleService.HandleException(ex); 
            }
              
        }

        //API Endpoint for deleting an employee by ID
        [HttpDelete("delete/{id}")] 
        public async Task<IActionResult> DeleteAsync(int id)
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
                // Checks if the provided ID is valid
                if (id <= 0)
                {
                    // Sets StatusCode to 400 indicating a bad request
                    response.StatusCode = 400;
                    response.Message = "Id Required.";

                    // Returns BadRequest response with the error message
                    return BadRequest(response);
                }

                // Calls the manager to asynchronously delete the employee by ID
                await _manager.DeleteAsync(id);
                response.StatusCode=200;
                response.Message = Constants.deleted;

                // Returns Ok response with the success message
                return Ok(response);

            }
            catch(Exception ex)
            {
                // Handle any exceptions using the provided exception handling service.
                return await _exceptionHandleService.HandleException(ex);  
            }
        }


        // API Endpoint for retrieving all job employee details
        [HttpGet("getall-job-employee")] 
        public async Task<IActionResult> GetAllJobEmployeeAsync(int jobId)
        {
            // Checks if the model state is valid
            if (!ModelState.IsValid)
            {
                // Returns a BadRequest response with a list of errors if model state is not valid
                return BadRequest(ModelState.GetErrorList()); 
            }

            // Initializes the response object for returning the result with pagination support
            var response = new BaseResponsePagination<object>();

            try
            {
                // Retrieves all job employee details asynchronously by the provided job ID
                var data = await _manager.GetAllJobEmployeeAsync(jobId);

                // Checks if data is not null
                if (data.adminSelect != null)
                {
                    // Sets StatusCode to 200 indicating success
                    response.StatusCode = 200;
                    response.Data = data.adminSelect;
                    response.TotalCount = data.totalCount;

                    // Returns Ok response with the data
                    return Ok(response);
                }

                // Sets StatusCode to 400 indicating that the request is invalid or cannot be processed
                response.StatusCode = 400;
                response.Message = Constants.error;

                // Returns BadRequest response with the error message
                return BadRequest(response);

            }

            catch (Exception ex)
            {
                // Handle any exceptions using the provided exception handling service.
                return await _exceptionHandleService.HandleException(ex);
            }
        }


        //API Endpoint to Accept Reject 
        [HttpPost("EmployerAcceptReject")]
        public async Task<IActionResult> EmployerAcceptReject(int jobId, int employeeId)
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
               if(employeeId <= 0 && jobId <= 0)
               {
                    response.StatusCode = 400;
                    response.Message = "Id Required";
                    return BadRequest(response);
               }

                string result = await _manager.AcceptRejectAsync(jobId, employeeId);

                /*
                var token = await _tokenManager.FcmTokenByRole("admin");

                foreach (var t in token)
                {
                    string body = $"employer {result}";
                    await _sendNotificationService.SendNotification("Employer Accepted", body, t.TokenFCM);
                }


                var token1 = await _tokenManager.FcmTokenByRole("employee");

                foreach (var y in token1)
                {
                    string body = $"employer {result} ";
                    await _sendNotificationService.SendNotification("", body, y.TokenFCM);
                }*/

                /*var adminToken = await _tokenManager.FcmTokenByRole("admin");
                string adminMessage = $"employer {result} ";*/

                /* var employeeToken = await _tokenManager.FcmTokenByRole("employee");
                 string employeeMessage = $"employer {result} ";*/


                if (result == "Accepted")
                {
                    var adminToken = await _tokenManager.FcmTokenByRole("admin");
                    string adminMessage = $"employer{result}";

                    if (adminToken != null)
                    {
                        await _sendNotificationService.SendNotification(" Acknowledgement of Employment Acceptance", adminMessage, adminToken.Select(t => t.TokenFCM).FirstOrDefault());
                    }

                    var employeeToken = await _tokenManager.FcmTokenByRole("employee");
                    string employeeMessage = $"employer {result}";

                    if (employeeToken != null)
                    {
                        await _sendNotificationService.SendNotification(" Acknowledgement of Employment Acceptance", employeeMessage, employeeToken.Select(t => t.TokenFCM).FirstOrDefault());
                    }
                }



                response.StatusCode = 200;
                response.Message = $" {result} Succesfully! ";
                return Ok(response);
            }
            catch(Exception ex)
            {
                // Handle the exception using the provided exception handling service.
                return await _exceptionHandleService.HandleException(ex);
            }
        }

        [HttpPost("Status")]
        public async Task<IActionResult> EmployerStatus(SetAdminSelectEmployeeModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }

            var response = new BaseResponseAdd();

            try
            {
                if(model.EmployeeId <= 0)
                {
                    response.StatusCode = 400;
                    response.Message = "Id Required!";
                    return BadRequest(response);
                }

                string result = await _manager.SetStatus(model);
                response.StatusCode = 200;
                response.Message = $" {result} Now!!";
                return Ok(response);
            }
            catch(Exception ex)
            {
                return await _exceptionHandleService.HandleException(ex);
            }
        }
    }
}