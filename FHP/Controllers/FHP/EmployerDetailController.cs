using FHP.infrastructure.DataLayer;
using FHP.infrastructure.Manager.FHP;
using FHP.infrastructure.Service;
using FHP.models.FHP.EmployerDetail;
using FHP.utilities;
using Microsoft.AspNetCore.Mvc;

namespace FHP.Controllers.FHP
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployerDetailController : ControllerBase
    {
        private readonly IEmployerDetailManager _manager;
        private readonly IExceptionHandleService _exceptionHandleService;
        private readonly IFileUploadService _fileUploadService;
        private readonly IUnitOfWork _unitOfWork;

        public EmployerDetailController(IEmployerDetailManager manager,
            IExceptionHandleService exceptionHandleService,
            IUnitOfWork unitOfWork,
            IFileUploadService fileUploadService)
        {
            _manager = manager;
            _exceptionHandleService = exceptionHandleService;
            _unitOfWork = unitOfWork;
            _fileUploadService = fileUploadService;
        }


        [HttpPost("add")]
        public async Task<IActionResult> AddAsync([FromForm] AddEmployerDetailModel model)
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
                // Check if the required fields are provided
                if (model.Id == 0 && model.UserId != 0 && model.CityId != 0 && model.CountryId != 0 && model.StateId != 0
                    && !string.IsNullOrEmpty(model.NationalAddress)
                    && !string.IsNullOrEmpty(model.ContactId)
                    && !string.IsNullOrEmpty(model.CompanyLogoURL)
                    && !string.IsNullOrEmpty(model.Telephone)
                    && !string.IsNullOrEmpty(model.Fax)
                    && !string.IsNullOrEmpty(model.TypeOfBusiness)
                    && !string.IsNullOrEmpty(model.PrincipalBusinessActivity)
                    && !string.IsNullOrEmpty(model.PersonToContact)
                    && !string.IsNullOrEmpty(model.WebAddress))
                {
                    // Upload certificate registration and VAT certificate URLs if provided
                    string certificateRegistration = string.Empty;

                    string vatCertificate = string.Empty;

                    if (model.CertificateRegistrationURL != null)
                    {
                        certificateRegistration = await _fileUploadService.UploadIFormFileAsync(model.CertificateRegistrationURL);
                    }

                    if (model.VATCertificateURL != null)
                    {
                        vatCertificate = await _fileUploadService.UploadIFormFileAsync(model.VATCertificateURL);
                    }

                    // Add the employer detail
                    await _manager.AddAsync(model, vatCertificate, certificateRegistration);
                    await transaction.CommitAsync();
                    response.StatusCode = 200;
                    response.Message = Constants.added;
                    return Ok(response);
                }

                response.StatusCode = 400;
                response.Message = Constants.provideValues;
                return BadRequest(response);

            }
            catch (Exception ex)
            {
                //In case of any exceptions during the process, it rolls back the transaction.
                await transaction.RollbackAsync();

                // Handle the exception using the provided exception handling service
                return await _exceptionHandleService.HandleException(ex);
            }
        }

        //Edit EmployerDetail
        [HttpPut("edit")]  
        public async Task<IActionResult> EditAsync([FromForm] AddEmployerDetailModel model)
        {
            // Check if the model state is valid
            if (!ModelState.IsValid)
            {
                // Return a BadRequest response with a list of errors if model state is not valid
                return BadRequest(ModelState.GetErrorList());
            }

            var response = new BaseResponseAdd();

            //The method then begins a database transaction to ensure data consistency during  updation.
            await using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                // Check if the provided model ID is valid
                if (model.Id >= 0)
                {
                    // Upload certificate registration and VAT certificate URLs if provided
                    string certificateRegistration = string.Empty;

                    string vatCertificate = string.Empty;

                    if (model.CertificateRegistrationURL != null)
                    {
                        certificateRegistration = await _fileUploadService.UploadIFormFileAsync(model.CertificateRegistrationURL);
                    }

                    if (model.VATCertificateURL != null)
                    {
                        vatCertificate = await _fileUploadService.UploadIFormFileAsync(model.VATCertificateURL);
                    }

                    await _manager.Edit(model, vatCertificate, certificateRegistration);
                    await transaction.CommitAsync();
                    response.StatusCode = 200;
                    response.Message = Constants.updated;
                    return Ok(response);

                }

                response.StatusCode = 400;
                response.Message = Constants.provideValues;
                return BadRequest(response);

            }
            catch (Exception ex)
            {
                //In case of any exceptions during the process, it rolls back the transaction.
                await transaction.RollbackAsync();

                // Handle the exception using the provided exception handling service
                return await _exceptionHandleService.HandleException(ex);
            }
        }

        [HttpGet("getall-pagination")]  
        public async Task<IActionResult> GetAllAsync(int page, int pageSize, int userId, string? search)
        {
            // Check if the model state is valid
            if (!ModelState.IsValid)
            {
                // Return a BadRequest response with a list of errors if model state is not valid
                return BadRequest(ModelState.GetErrorList());
            }

            var response = new BaseResponsePagination<object>();

            try
            {
                // Retrieve all employer details with pagination
                var data = await _manager.GetAllAsync(page, pageSize, userId, search);

                if (data.employerDetail != null && data.totalCount > 0)
                {
                    response.Data = data.employerDetail;
                    response.TotalCount = data.totalCount;
                    response.StatusCode = 200;
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

        [HttpGet("getbyid")] 
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            // Check if the model state is valid
            if (!ModelState.IsValid)
            {
                // Return a BadRequest response with a list of errors if model state is not valid
                return BadRequest(ModelState.GetErrorList()); 
            }

            var response = new BaseResponseAddResponse<object>();

            try
            {
                // Retrieve an employer detail by its ID
                var data = await _manager.GetByIdAsync(id);

                if (data != null)
                {
                    response.StatusCode = 200;
                    response.Data = data;
                    return Ok(response);
                }

                response.StatusCode = 400;
                response.Message = "no data avaiable";
                response.Data = "";
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                // Handle the exception using the provided exception handling service
                return await _exceptionHandleService.HandleException(ex);
            }
        }

        [HttpDelete("delete/{id}")]  
        public async Task<IActionResult> DeleteAsync(int id)
        {
            // Check if the model state is valid
            if (!ModelState.IsValid)
            {
                // Return a BadRequest response with a list of errors if model state is not valid
                return BadRequest(ModelState.GetErrorList());  
            }

            var response = new BaseResponseAdd();

            try
            {
                // Check if the provided ID is valid
                if (id <= 0)
                {
                    response.StatusCode = 400;
                    response.Message = "Id Required";
                    return BadRequest(response);
                }

                // Delete the employer detail
                await _manager.DeleteAsync(id);

                response.StatusCode = 200;
                response.Message = Constants.deleted;
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