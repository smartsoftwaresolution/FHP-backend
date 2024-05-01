using FHP.infrastructure.DataLayer;
using FHP.infrastructure.Manager.FHP;
using FHP.infrastructure.Manager.UserManagement;
using FHP.infrastructure.Service;
using FHP.models.FHP.JobPosting;
using FHP.utilities;
using Microsoft.AspNetCore.Mvc;

namespace FHP.Controllers.FHP
{ 
    [Route("api/[controller]")]
    [ApiController]
    public class JobPostingController : ControllerBase
    {
        private readonly IJobPostingManager _manager;
        private readonly IExceptionHandleService _exceptionHandleService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISendNotificationService _sendNotificationService;
        private readonly IFCMTokenManager _tokenManager;
        public JobPostingController(IJobPostingManager manager,
                                    IExceptionHandleService exceptionHandleService,
                                    IUnitOfWork unitOfWork,
                                    ISendNotificationService sendNotificationService,
                                    IFCMTokenManager tokenManager)
        {
            _manager = manager;
            _exceptionHandleService = exceptionHandleService;
            _unitOfWork = unitOfWork;
            _sendNotificationService = sendNotificationService;
            _tokenManager = tokenManager;
        }
                   
        // API endpoint to add jobposting 
        [HttpPost("add")]
        public async Task<IActionResult> AddAsync(AddJobPostingModel model)
        {
            // Checks if the model state is valid
            if (!ModelState.IsValid)
            {
                // Returns a BadRequest response with a list of errors if model state is not valid
                return BadRequest(ModelState.GetErrorList());
            }

            var response = new BaseResponseAdd();

            //The method then begins a database transaction to ensure data consistency during  addition.
            await using var transaction = await _unitOfWork.BeginTransactionAsync(); 

            try
            {
                // Checks if the provided model contains all required information
                if (model.Id == 0 && model.UserId != 0
                    && !string.IsNullOrEmpty(model.JobTitle)
                    && !string.IsNullOrEmpty(model.Description)
                    && !string.IsNullOrEmpty(model.Experience)
                    && !string.IsNullOrEmpty(model.RolesAndResponsibilities)
                    && !string.IsNullOrEmpty(model.Skills)
                    && !string.IsNullOrEmpty(model.Address)
                    && !string.IsNullOrEmpty(model.Payout))
                    
                {
                    // Adds the job posting asynchronously
                    await _manager.AddAsync(model);

                    if (model.JobPosting == Constants.JobPosting.Submitted)
                    {
                        var token = await _tokenManager.FcmTokenByRole("admin");

                        foreach (var t in token)
                        {
                            string body = "Dear Admin,\r\n\r\nA new job posting has been submitted. Please review the details and take appropriate action.\r\n\r\nThank you.";

                            await _sendNotificationService.SendNotificationAsync("A new job post ", body, t.TokenFCM);

                        }
                    }

                    // commit transaction
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
                //In case of any exceptions during the process, it rolls back the transaction.
                await transaction.RollbackAsync(); 

                // Handle the exception using the provided exception handling service
                return await _exceptionHandleService.HandleException(ex); 
            }
        }


        // API endpoint to edit jobposting 
        [HttpPut("edit")]
        public async Task<IActionResult> EditAsync(AddJobPostingModel model)
        {
            // Checks if the model state is valid
            if (!ModelState.IsValid)
            {
                // Returns a BadRequest response with a list of errors if model state is not valid
                return BadRequest(ModelState.GetErrorList());
            }

            var response = new BaseResponseAdd();

            //The method then begins a database transaction to ensure data consistency during  updation.
            await using var transaction = await _unitOfWork.BeginTransactionAsync();  

            try
            {
                // Checks if the provided model ID is valid
                if (model.Id >= 0)
                {
                    // Updates the job posting asynchronously
                    string message = await _manager.Edit(model); 

                    await transaction.CommitAsync(); 

                    if (message.Contains("updated successfully"))

                    { 
                        response.StatusCode = 200;
                        response.Message = message;
                        return Ok(response);
                    }
                    else
                    {
                        response.StatusCode = 400;
                        response.Message = message;
                        return BadRequest(response);
                    }
                }

                response.StatusCode = 400;
                response.Message = Constants.provideValues;
                return BadRequest(response);
            }
            catch(Exception ex)
            {
                //In case of any exceptions during the process, it rolls back the transaction.
                await transaction.RollbackAsync(); 

                // Handle the exception using the provided exception handling service
                return await  _exceptionHandleService.HandleException(ex); 
            } 
        }


        // API endpoint to gell all jobposting with search
        [HttpGet("getall-pagination")]
        public async Task<IActionResult> GetAllAsync(int page,int pageSize,string? search,int userId,bool? IsAdmin)
        {
            // Checks if the model state is valid
            if (!ModelState.IsValid)
            {
                // Returns a BadRequest response with a list of errors if model state is not valid
                return BadRequest(ModelState.GetErrorList());
            }

            var response = new BaseResponsePagination<object>();

            try
            {
                // Retrieves all job postings asynchronously
                var data = await _manager.GetAllAsync(page,pageSize,search,userId,IsAdmin);

                if (data.jobPosting != null)
                {
                    response.StatusCode = 200;
                    response.Data = data.jobPosting;
                    response.TotalCount = data.totalCount;
                    return Ok(response);
                }

                response.StatusCode = 400;
                response.Message = Constants.error;
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                // Handle the exception using the provided exception handling service
                return await _exceptionHandleService.HandleException(ex); 
            }
        }


        // API endpoint to get by id  
        [HttpGet("getbyid")]
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
                // Retrieves a job posting by its ID asynchronously
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
                // Handle the exception using the provided exception handling service
                return await _exceptionHandleService.HandleException(ex);
            }
        }


        // API endpoint to delete jobposting 
        [HttpDelete("delete/{id}")]
        public  async Task<IActionResult> DeleteAsync(int id)
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
                // Checks if the provided ID is valid
                if (id <= 0)
                {
                    response.StatusCode = 400;
                    response.Message = "Id Required.";
                    return BadRequest(response);
                }

                await _manager.DeleteAsync(id);
                response.StatusCode = 200;
                response.Message = Constants.deleted;
                return Ok(response);

            }
            catch(Exception ex)
            {
                // Handle the exception using the provided exception handling service
                return await _exceptionHandleService.HandleException(ex); 
            }
        }


        // Activate - decativate JobPosting
        [HttpPatch("active-deactive/{id}")]
        public async Task<IActionResult> ActiveDeactiveAsync(int id)
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
                // Checks if the provided ID is valid
                if (id <= 0)
                {
                    response.StatusCode = 400;
                    response.Message = "Id Required.";
                    return BadRequest(response);
                }

                // Activates or deactivates the job posting asynchronously
                string result =  await _manager.ActiveDeactiveAsync(id);

                response.StatusCode = 200;
                response.Message = $"Job {result} Successfully!!!";
                return Ok(response);

            }
            catch (Exception ex)
            {
                // Handle the exception using the provided exception handling service
                return await _exceptionHandleService.HandleException(ex); 
            }
        }


        // Submit JobPosting
        [HttpPatch("submit-job/{id}")]
        public async Task<IActionResult> SubmitJobAsync(int id)
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
                // Checks if the provided ID is valid
                if (id <= 0)
                {
                    response.StatusCode = 400;
                    response.Message = "Id Required.";
                    return BadRequest(response);
                }

                // Submits the job posting asynchronously
                await _manager.SubmitJobAsync(id);
                response.StatusCode = 200;
                response.Message = $"Job Submitted Successfully!!!";
                return Ok(response);

            }
            catch (Exception ex)
            {
                // Handle the exception using the provided exception handling service
                return await _exceptionHandleService.HandleException(ex);
            }
        }



        // Cancel jobposting
        [HttpPatch("cancel-job/{id}")]
        public async Task<IActionResult> CancelJobAsync(int id, string cancelReason)
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
                // Checks if the provided ID is valid
                if (id <= 0)
                {
                    response.StatusCode = 400;
                    response.Message = "Id Required.";
                    return BadRequest(response);
                }

                // Cancels the job posting asynchronously with the provided cancellation reason
                await _manager.CancelJobAsync(id, cancelReason);
                response.StatusCode = 200;
                response.Message = $"Job Cancel Successfully!!!";
                return Ok(response);

            }
            catch (Exception ex)
            {
                // Handle the exception using the provided exception handling service
                return await _exceptionHandleService.HandleException(ex);
            }
        }



        [HttpPatch("set-job-processing-status/{id}")]
        public async Task<IActionResult> SetJobProcessingStatusAsync(int id, Constants.JobProcessingStatus jobProcessingStatus)
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
                // Checks if the provided ID is valid
                if (id <= 0)
                {
                    response.StatusCode = 400;
                    response.Message = "Id Required.";
                    return BadRequest(response);
                }

                // Sets job processing status for the job posting asynchronously
                await _manager.SetJobProcessingStatus(id, jobProcessingStatus);

                response.StatusCode = 200;
                response.Message = $"Job Processing Status set Successfully!!!";
                return Ok(response);
            }
            catch (Exception ex)
            {
                // Handle the exception using the provided exception handling service
                return await _exceptionHandleService.HandleException(ex); 
            } 
        }
    }
}