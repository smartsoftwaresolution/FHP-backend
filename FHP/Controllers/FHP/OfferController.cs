using FHP.infrastructure.DataLayer;
using FHP.infrastructure.Manager.FHP;
using FHP.infrastructure.Manager.UserManagement;
using FHP.infrastructure.Service;
using FHP.models.FHP.Offer;
using FHP.utilities;
using Microsoft.AspNetCore.Mvc;

namespace FHP.Controllers.FHP
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfferController : ControllerBase
    {
        private readonly IOfferManager _manager;   
        private readonly IExceptionHandleService _exceptionHandleService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFCMTokenManager _fCMTokenManager;
        private readonly ISendNotificationService _sendNotificationService;

        public OfferController(IOfferManager manager,
                              IExceptionHandleService exceptionHandleService,
                              IUnitOfWork unitOfWork,
                              IFCMTokenManager fCMTokenManager,
                              ISendNotificationService sendNotificationService)
        {
            _manager  = manager;
            _exceptionHandleService = exceptionHandleService;
            _unitOfWork = unitOfWork;
            _fCMTokenManager = fCMTokenManager;
            _sendNotificationService = sendNotificationService;
        }

        // API endpoint to add Offer 
        [HttpPost("add")]
        public async Task<IActionResult> AddAsync(AddOfferModel model)
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
                if (model.Id == 0 && model.JobId != 0 && model.EmployeeId != 0 && model.EmployerId != 0 &&
                    !string.IsNullOrEmpty(model.Title) &&
                    !string.IsNullOrEmpty(model.Description))
                {

                    await _manager.AddAsync(model);

                    var admintoken = await _fCMTokenManager.FcmTokenByRole("admin");
                    var token = admintoken.OrderByDescending(s => s.Id).FirstOrDefault();

                    var employeetoken = await _fCMTokenManager.FcmTokenByRole("employee");
                    var token1 = employeetoken.OrderByDescending(s => s.Id).FirstOrDefault();

                    if (token != null)
                    {
                        string adminbody = "An offer has been sent successfully!";
                        await _sendNotificationService.SendNotification("Offer", adminbody, token.TokenFCM);
                    }

                    if (token1 != null)
                    {
                        string employeebody = "An a offer has been sent successfully!";
                        await _sendNotificationService.SendNotification("Offer", employeebody, token1.TokenFCM);
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
            catch (Exception ex)
            {
                // In case of any exceptions during the process, roll back the transaction.
                await transaction.RollbackAsync();

                // Handle the exception using the provided exception handling service.
                return await _exceptionHandleService.HandleException(ex);
            }
        }



        // API endpoint to edit Offer 
        [HttpPut("edit")]
        public async Task<IActionResult> EditAsync(AddOfferModel model)
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
                if (model.Id >= 0)
                {
                    // Edit the Offer  model asynchronously.
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

        // API endpoint to get all Offer 
        [HttpGet("getall-pagination")]
        public async Task<IActionResult> GetAllAsync(int page, int pageSize,string? search,int employeeId,int employerId)
        {
            // If the model state is not valid, return a BadRequest response with a list of errors.
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }

            var response = new BaseResponsePagination<object>();

            try
            {
                var data = await _manager.GetAllAsync(page,pageSize,search,employeeId,employerId);

                if(data.offer != null )
                {
                    response.StatusCode = 200;
                    response.Data = data.offer;
                    response.TotalCount = data.totalCount;
                    return Ok(response);    
                }

                response.StatusCode = 400;
                response.Message = Constants.error;
                return BadRequest(response);
            }
            catch(Exception ex)
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
                response.Message = Constants.error;
                return BadRequest(response);
            }

            catch (Exception ex)
            {
                // Handle any exceptions using the provided exception handling service.
                return await _exceptionHandleService.HandleException(ex);
            }

        }

        //API Endpoint for deleting an Offer by ID
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

                // Calls the manager to asynchronously delete the Offer by ID
                await _manager.DeleteAsync(id);
                response.StatusCode = 200;
                response.Message = Constants.deleted;

                // Returns Ok response with the success message
                return Ok(response);

            }
            catch (Exception ex)
            {
                // Handle any exceptions using the provided exception handling service.
                return await _exceptionHandleService.HandleException(ex);
            }
        }
/*
        [HttpPost("OfferAcceptReject")]
        public async Task<IActionResult> OfferAcceptRejectAsync(int id,int jobId,int employeeId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }

            var response = new BaseResponseAdd();

            try
            {
                if(id <= 0 && jobId <= 0 && employeeId <= 0)
                {
                    response.StatusCode = 400;
                    response.Message = "Id Required";
                    return BadRequest(response);    
                }

                string result = await _manager.OfferAcceptRejectAsync(id, jobId, employeeId);

                response.StatusCode = 200;
                response.Message = $"{result} Succesfully!";
                return Ok(response);

            }
            catch(Exception ex)
            {
                return await _exceptionHandleService.HandleException(ex);
            }
        }*/

        [HttpPost("OfferAcceptReject")]
        public async Task<IActionResult> AcceptRejectAsync(SetOfferStatusModel model)
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
                    response.Message = "Id Required";
                    return BadRequest(response);
                }


                    string result = await _manager.OfferAcceptRejectAsync(model);

                    var adminToken = await _fCMTokenManager.FcmTokenByRole("admin");
                    var Token = adminToken.OrderByDescending(a => a.Id).FirstOrDefault();

                    var employerToken = await _fCMTokenManager.FcmTokenByRole("employer");
                    var tokens = employerToken.OrderByDescending(e => e.Id).FirstOrDefault();

                if (result == "Accepted")
                {
                    if (Token != null)
                    {
                        string adminMessage = "The offer has been accepted by the employee. Please proceed accordingly.";
                        await _sendNotificationService.SendNotification("Offer Accept", adminMessage, Token.TokenFCM);
                    }

                    if (tokens != null)
                    {
                        string employerMessage = "The offer has been accepted by the employee. Please proceed accordingly.";
                        await _sendNotificationService.SendNotification("Offer Accept", employerMessage, tokens.TokenFCM);
                    }
                }


                response.StatusCode = 200;
                response.Message = $"Offer {result} Succesfully!!";
                return Ok(response);
            }
            catch(Exception ex)
            {
                return await _exceptionHandleService.HandleException(ex);   
            }
        }
    }
}
