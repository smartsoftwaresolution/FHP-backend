using FHP.infrastructure.Manager.FHP;
using FHP.infrastructure.Service;
using FHP.models.FHP;
using FHP.services;
using FHP.utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace FHP.Controllers.FHP
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployerDetailController : ControllerBase
    {
        private readonly IEmployerDetailManager _manager;
        private readonly IExceptionHandleService _exceptionHandleService;

        public EmployerDetailController(IEmployerDetailManager manager,IExceptionHandleService exceptionHandleService)
        {
                _manager=manager;
                _exceptionHandleService=exceptionHandleService;
        }


        [HttpPost("add")]
        public async Task<IActionResult> AddAsync(AddEmployerDetailModel model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }

            var response = new BaseResponseAdd();

            try
            {
<<<<<<< HEAD
                if(model.Id == 0 && model.UserId != 0 && 
                    model.CityId != 0 && model.CountryId != 0 && model.StateId != 0)
=======
                if(model.Id == 0 && model.UserId != 0 && model.CityId != 0 && model.CountryId != 0 && model.StateId != 0 
                    && !string.IsNullOrEmpty(model.NationalAddress)
                    && !string.IsNullOrEmpty(model.CertificateRegistrationURL)
                    && !string.IsNullOrEmpty(model.VATCertificateURL)
                    && !string.IsNullOrEmpty(model.ContactId)
                    && !string.IsNullOrEmpty(model.CompanyLogoURL)
                    && !string.IsNullOrEmpty(model.Telephone)
                    && !string.IsNullOrEmpty(model.Fax)
                    && !string.IsNullOrEmpty(model.TypeOfBusiness)
                    && !string.IsNullOrEmpty(model.PrincipalBusinessActivity)
                    && !string.IsNullOrEmpty(model.PersonToContact)
                    && !string.IsNullOrEmpty(model.WebAddress))
>>>>>>> 06ea6358a4abed4c85fd897205fb005dda77b445
                {

                    await _manager.AddAsync(model);
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
                return await _exceptionHandleService.HandleException(ex);
            }
        }

        [HttpPut("edit")]
        public async Task<IActionResult> EditAsync(AddEmployerDetailModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }

            var response = new BaseResponseAdd();

            try
            {
                if(model.Id >= 0)
                {
                    await _manager.Edit(model);

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
                return await _exceptionHandleService.HandleException(ex);
            }
        }

        [HttpGet("getall-pagination")]
        public async Task<IActionResult> GetAllAsync(int page ,int pageSize,int userId,string? search)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
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
               return await _exceptionHandleService.HandleException(ex);
           }
        }

        [HttpGet("getbyid")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
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
                return await _exceptionHandleService.HandleException(ex);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
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
                return await _exceptionHandleService.HandleException(ex);
            }
        }
    }
}
