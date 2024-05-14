using FHP.infrastructure.DataLayer;
using FHP.infrastructure.Manager.FHP;
using FHP.infrastructure.Manager.UserManagement;
using FHP.infrastructure.Service;
using FHP.models.FHP.Contract;
using FHP.utilities;
using Microsoft.AspNetCore.Mvc;

namespace FHP.Controllers.FHP
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractController : ControllerBase
    {
        private readonly IContractManager _manager;
        private readonly IExceptionHandleService _exceptionHandleService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISendNotificationService _sendNotificationService;
        private readonly IFCMTokenManager _fCMTokenManager;

        public ContractController(IContractManager manager, 
                                  IExceptionHandleService exceptionHandleService,
                                  IUnitOfWork unitOfWork,
                                  ISendNotificationService sendNotificationService,
                                  IFCMTokenManager fCMTokenManager)
        {
            _manager=manager;
            _exceptionHandleService = exceptionHandleService;
            _unitOfWork = unitOfWork;
            _sendNotificationService = sendNotificationService;
            _fCMTokenManager = fCMTokenManager;
        }


        // API endpoint to add Contract
        [HttpPost("add")]   
        public async Task<IActionResult> AddAsync(AddContractModel model)
        {
            if (!ModelState.IsValid)
            {
                //it returns a BadRequest response with a list of errors.
                return BadRequest(ModelState.GetErrorList()); 
            }

            var response = new BaseResponseAdd();

            //The method then begins a database transaction to ensure data consistency during  addition.
            await using var transaction = await _unitOfWork.BeginTransactionAsync(); 

            try
            {
                if (model.Id == 0 && model.EmployeeId != 0 && model.JobId != 0 && model.EmployerId != 0
                    && !string.IsNullOrEmpty(model.Description)
                    && !string.IsNullOrEmpty(model.EmployeeSignature)
                    && !string.IsNullOrEmpty(model.EmployerSignature))

                {
                    // Add the contract model asynchronously.
                    await _manager.AddAsync(model);

                  

                    var adminToken = (await _fCMTokenManager.FcmTokenByRole("admin")).DistinctBy(t => t.TokenFCM);

                    var token = adminToken.FirstOrDefault();

                    if(token != null)
                    {
                        string adminMessage = "Hello, A contract has been signed by both employer and employee";
                        await _sendNotificationService.SendNotification("New Contract Notification", adminMessage, token.TokenFCM);
                    }

                    var employeeToken = (await _fCMTokenManager.FcmTokenByRole("employee")).DistinctBy(t => t.TokenFCM);
                    var tokens = employeeToken. FirstOrDefault();

                    if(tokens != null)
                    {
                        string employeeMessage = "Hello A contract has been signed";
                        await _sendNotificationService.SendNotification("New contract notification", employeeMessage, tokens.TokenFCM);
                    }

                   /* var employeeToken = await _fCMTokenManager.FcmTokenByRole("employee");

                    var allTokens = adminToken.Concat(employeeToken).Select(t => t.TokenFCM).FirstOrDefault();

                    if (allTokens != null)
                    {
                      string message = "A new contract has been created. Please review the details at your earliest convenience.";

                      await _sendNotificationService.SendNotification("New Contract Notification", message, allTokens);
                    }*/

                    /* string adminbody = "I hope this message finds you well. I am writing to inform you about a new contract that has been created Please review the details of the contract at your earliest convenience.";

                     string employeebody = "I trust this message finds you well. I am writing to inform you that a new contract has been created Please carefully examine the terms and conditions outlined in the contract.. Should you have any questions or require clarification on any aspect, do not hesitate to reach out to me.";

                     await _sendNotificationService.SendNotification("Dear sir a new contract has been created", adminbody, adminToken.Select(t => t.TokenFCM).FirstOrDefault());

                     await _sendNotificationService.SendNotification("Dear sir a new contract has been created", adminbody, employeeToken.Select(t => t.TokenFCM).FirstOrDefault());*/

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
            catch (Exception ex)
            {
                // In case of any exceptions during the process, roll back the transaction.
                await transaction.RollbackAsync();

                // Handle the exception using the provided exception handling service.
                return await _exceptionHandleService.HandleException(ex);  
            }
        }


        // API endpoint to edit Contract 
        [HttpPut("edit")]  
        public async Task<IActionResult> EditAsync(AddContractModel model)
        {
            if (!ModelState.IsValid)
            {
                //it returns a BadRequest response with a list of errors.
                return BadRequest(ModelState.GetErrorList()); 
            }
           
            // Response object to be sent back.
            var response = new BaseResponseAdd();

            // Begin a database transaction to ensure data consistency during updation.
            await using var transaction = await _unitOfWork.BeginTransactionAsync(); 

            try
            {
                if(model.Id >= 0)
                {
                    // Edit the Contract model asynchronously.
                    await _manager.Edit(model);

                    // Commit the transaction.
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
                // In case of any exceptions during the process, roll back the transaction.
                await transaction.RollbackAsync();

                // Handle the exception using the provided exception handling service.
                return await _exceptionHandleService.HandleException(ex); 
            }
        }

        // Get All Contract with Pagination and search filter
        [HttpGet("getall-pagination")] 
        public async Task<IActionResult> GetAllAsync(int page,int pageSize,string? search,int employeeId,int employerId)
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
                var data = await _manager.GetAllAsync(page,pageSize,search,employeeId,employerId);

                // Check if data is retrieved successfully.
                if (data.contract != null)
                {
                    response.StatusCode = 200;
                    response.Data = data.contract;
                    response.TotalCount = data.totalCount;
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


        // Get By Id Contract 
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
                // Retrieve Contract data by its Id from the manager.
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
            catch(Exception ex)
            {
                // Handle any exceptions using the provided exception handling service.
                return await _exceptionHandleService.HandleException(ex);
            }
        }

        // Delete Contract by Id
        [HttpDelete("delete/{id}")] 
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                //it returns a BadRequest response with a list of errors
                return BadRequest(ModelState.GetErrorList()); 
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
                response.StatusCode = 200;
                response.Message = Constants.deleted;
                return Ok(response); 

            }
            catch(Exception ex)
            {
                //exceptionHandle service.
                return await _exceptionHandleService.HandleException(ex); 
            }
        }
    }
}
