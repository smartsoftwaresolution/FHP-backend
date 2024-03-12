using FHP.infrastructure.DataLayer;
using FHP.infrastructure.Manager.FHP;
using FHP.infrastructure.Service;
using FHP.models.FHP;
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
                _fileUploadService=fileUploadService;   
        }


        [HttpPost("add")]  //add EmployerDetail
        public async Task<IActionResult> AddAsync([FromForm]AddEmployerDetailModel model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList()); //it returns a BadRequest response with a list of errors.
            }

            var response = new BaseResponseAdd();

            await using var transaction = await _unitOfWork.BeginTransactionAsync(); //The method then begins a database transaction to ensure data consistency during  addition.
            try
            {
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

                    string certificateRegistration = string.Empty;

                    string vatCertificate = string.Empty;

                    if(model.CertificateRegistrationURL != null)
                    {
                        certificateRegistration = await _fileUploadService.UploadIFormFileAsync(model.CertificateRegistrationURL); // upload CertificateRegistrationURL service
                    }

                    if(model.VATCertificateURL != null)
                    {
                        vatCertificate = await _fileUploadService.UploadIFormFileAsync(model.VATCertificateURL); // upload VATCertificateURL service
                    }

                    await _manager.AddAsync(model,vatCertificate,certificateRegistration); // added
                    await transaction.CommitAsync(); //commit transaction
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
                await transaction.RollbackAsync();  //In case of any exceptions during the process, it rolls back the transaction.
                return await _exceptionHandleService.HandleException(ex); //exception hadler service
            }
        }

        [HttpPut("edit")]  //Edit EmployerDetail
        public async Task<IActionResult> EditAsync([FromForm]AddEmployerDetailModel model)
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

                    string certificateRegistration = string.Empty;

                    string vatCertificate = string.Empty;

                    if (model.CertificateRegistrationURL != null)
                    {
                        certificateRegistration = await _fileUploadService.UploadIFormFileAsync(model.CertificateRegistrationURL);   // upload CertificateRegistrationURL service
                    }

                    if (model.VATCertificateURL != null)
                    {
                        vatCertificate = await _fileUploadService.UploadIFormFileAsync(model.VATCertificateURL);  // // upload VATCertificateURL service
                    }

                    await _manager.Edit(model,vatCertificate,certificateRegistration); //updated
                    await transaction.CommitAsync(); // commit transaction
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
                await transaction.RollbackAsync(); //In case of any exceptions during the process, it rolls back the transaction.
                return await _exceptionHandleService.HandleException(ex); //exception handler service
            }
        }

        [HttpGet("getall-pagination")]  // get all EmployerDetail
        public async Task<IActionResult> GetAllAsync(int page ,int pageSize,int userId,string? search)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList()); //it returns a BadRequest response with a list of errors.
            }
            var response = new BaseResponsePagination<object>();
            try
            {
                var data = await _manager.GetAllAsync(page ,pageSize,userId,search);
                if(data.employerDetail != null)
                {
                    response.StatusCode = 200;
                    response.Data = data.employerDetail;
                    response.TotalCount = data.totalCount;
                    return Ok(response);
                }
                response.StatusCode = 400;
                response.Message = Constants.error;
                return BadRequest(response);
            }
            catch(Exception ex)
            {
               return await _exceptionHandleService.HandleException(ex); // exceptionHandler service
            }
        }

        [HttpGet("getbyid")] // get by id EmployerDetail
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList()); //it returns a BadRequest response with a list of errors.
            }
            var response = new BaseResponseAddResponse<object>();
            try
            {
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
            catch (Exception ex)
            {
                return await _exceptionHandleService.HandleException(ex); // exceptionHandler service
            }
        }

        [HttpDelete("delete/{id}")]  //delete EmployerDetail
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
                    response.StatusCode = 400;
                    response.Message = "Id Required";
                    return BadRequest(response);
                }
                await _manager.DeleteAsync(id);
                response.StatusCode = 200;
                response.Message = Constants.deleted;
                return Ok(response);
            }
            catch(Exception ex)
            {
                return await _exceptionHandleService.HandleException(ex); // exceptionHandler service
            }
        }
    }
}
