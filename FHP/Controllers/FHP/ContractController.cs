using FHP.entity.FHP;
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
        private readonly IUserManager _userManager;
        private readonly IEmailService _emailService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly INotificationService _notificationService;
        private readonly IFileUploadService _fileUploadService;
        public ContractController(IContractManager manager, 
                                  IExceptionHandleService exceptionHandleService,
                                  IUnitOfWork unitOfWork,
                                  ISendNotificationService sendNotificationService,
                                  IFCMTokenManager fCMTokenManager,
                                  IUserManager userManager,
                                  IEmailService emailService,
                                  IWebHostEnvironment webHostEnvironment,
                                  INotificationService notificationService,
                                  IFileUploadService fileUploadService)
                                  
        {
            _manager = manager;
            _exceptionHandleService = exceptionHandleService;
            _unitOfWork = unitOfWork;
            _sendNotificationService = sendNotificationService;
            _fCMTokenManager = fCMTokenManager;
            _userManager = userManager;
            _emailService = emailService;
            _webHostEnvironment = webHostEnvironment;
            _notificationService = notificationService;
            _fileUploadService = fileUploadService;
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

            var response = new BaseResponseContractAdd();

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
                   var data =  await _manager.AddAsync(model);

                   await _notificationService.SendContractNotificationAsync();
                    
                   // Commit the transaction. 

                    await transaction.CommitAsync();  
                    response.StatusCode = 200;
                    response.Message = Constants.added;
                    response.Id = data;
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


                    var employertoken = await _fCMTokenManager.FcmTokenByRole("employer");

                    var token = employertoken.OrderByDescending(e => e.Id).FirstOrDefault();

                    if (token != null && !string.IsNullOrEmpty(model.EmployeeSignature))
                    {
                        string employerMessage = "A contract has been signed by employee.";
                        await _sendNotificationService.SendNotification("contract signed", employerMessage, token.TokenFCM);
                    }


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


        [HttpPost("contractSend")]
        public async Task<IActionResult> ContractSend(PostContractModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }

            var response = new BaseResponseAdd();

            try
            {
                var employeeEmail = await _userManager.GetByIdAsync(model.userId);

                if(model.Id == 0 && model.userId != 0 &&
                    employeeEmail != null && !string.IsNullOrEmpty(employeeEmail.Email))
                {
                  
                    await _emailService.SendContractEmail(employeeEmail.Email, employeeEmail.Id,model.HtmlContext,model.Subject);

                    return Ok(new
                    {
                        statusCode = 200,
                        Message = "send."
                    });
                }

                else
                {
                    return Ok(new { badRequest = 404, message = "Email Not found."});
                }
                
                
            }
            catch(Exception ex)
            {
                return BadRequest(_exceptionHandleService.HandleException(ex)); 
            }

        }


        [HttpPatch("upload-pdf")]
        public async Task<IActionResult> UploadPdfAsync(int id, IFormFile pdffile)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());   
            }

            await using var transaction = await _unitOfWork.BeginTransactionAsync();

            var response = new BasePdfResponse();

            try
            {
                if(id < 0)
                {
                    response.StatusCode = 400;
                    response.Message = Constants.provideValues;
                    return BadRequest(response);
                }

                var contractExists = await _manager.GetByIdAsync(id);
                if (contractExists == null)
                {
                    response.StatusCode = 404;
                    response.Message = "Contract not found.";
                    return NotFound(response);
                }

                if (pdffile == null)
                {
                    response.StatusCode = 400;
                    response.Message = "No PDF file provided.";
                    return BadRequest(response);
                }



                var existingPdfUrl = await _manager.GetPdfUrlByContractIdAsync(id);
                if (!string.IsNullOrEmpty(existingPdfUrl))
                {
                    /*var deleteExistsFile = await _fileUploadService.DeleteIFormPdfAsync(existingPdfUrl);
                    if (!deleteExistsFile)
                    {
                        response.StatusCode = 500;
                        response.Message = "Failed to delete existing PDF file.";
                        return BadRequest(response);
                    }*/
                }



                var file = await _fileUploadService.UploadIFormPdfAsync(pdffile);

                if (string.IsNullOrEmpty(file))
                {
                    response.StatusCode = 500;
                    response.Message = "Failed to upload PDF file.";
                    return BadRequest(response);
                }

                await _manager.AddPdfFile(id, file);

                await transaction.CommitAsync();

                response.StatusCode = 200;
                response.Message = "Pdf save sucessfully!";
                response.PdfUrl = file;
                return Ok(response);

            }
            catch(Exception ex)
            {
                await transaction.RollbackAsync();

                return await _exceptionHandleService.HandleException(ex);   
            }
        }

    } 
}
