
using FHP.infrastructure.Manager.FHP;
using FHP.infrastructure.Service;
using FHP.models.FHP;
using FHP.utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace FHP.Controllers.FHP
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeDetailController : ControllerBase
    {
        private readonly IEmployeeDetailManager _manager;
        private readonly IExceptionHandleService _exceptionHandleService;

        public EmployeeDetailController(IEmployeeDetailManager manager, IExceptionHandleService exceptionHandleService)
        {
            _manager = manager;
            _exceptionHandleService = exceptionHandleService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddAsync(AddEmployeeDetailModel model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }

            var response = new BaseResponseAdd();

            try
            {
                if(model.Id ==0 && model.UserId !=0 && model.CountryId !=0 && model.StateId!=0 && model.CityId !=0
                   && !string.IsNullOrEmpty(model.MaritalStatus)
                   && !string.IsNullOrEmpty(model.Gender)
                   && !string.IsNullOrEmpty(model.ResumeURL)
                   && !string.IsNullOrEmpty(model.ProfileImgURL)
                   && !string.IsNullOrEmpty(model.Hobby)
                   && !string.IsNullOrEmpty(model.PermanentAddress)
                   && !string.IsNullOrEmpty(model.AlternateAddress)
                   && !string.IsNullOrEmpty(model.Mobile)
                   && !string.IsNullOrEmpty(model.Phone)
                   && !string.IsNullOrEmpty(model.AlternatePhone)
                   && !string.IsNullOrEmpty(model.AlternateEmail)
                   && !string.IsNullOrEmpty(model.EmergencyContactNumber)
                   && !string.IsNullOrEmpty(model.EmergencyContactName))
                {
                    await _manager.AddAsync(model);
                    response.StatusCode = 200;
                    response.Message = Constants.added;
                    return Ok(response);
                }

                response.StatusCode = 400;
                response.Message= Constants.provideValues;
                return BadRequest(response);

            }
            catch(Exception ex) 
            { 
                return await _exceptionHandleService.HandleException(ex);
            }
        }


        [HttpPut("edit")]
        public async Task<IActionResult> EditAsync(AddEmployeeDetailModel model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }

            var response = new BaseResponseAdd();

            try
            {
                if(model.Id>=0)
                {
                    await _manager.Edit(model);
                    response.StatusCode = 200;
                    response.Message = Constants.updated;
                    return Ok(response);
                }


                response.StatusCode = 400;
                response.Message= Constants.error;
                return BadRequest(response);

            }
            catch(Exception ex)
            {
                return await _exceptionHandleService.HandleException(ex);
            }
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAllAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }

            var response = new BaseResponseAddResponse<object>();

            try
            {
                var data = await _manager.GetAllAsync();
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
                return await _exceptionHandleService.HandleException(ex);
            }
        }

        [HttpGet("getbyid")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            if(!ModelState.IsValid)
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
            catch(Exception ex)
            {
                return await _exceptionHandleService.HandleException(ex);
            }
        }


        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());   
            }

            var response = new BaseResponseAdd();

            try
            {
                if(id <= 0)
                {
                    response.StatusCode = 400;
                    response.Message = "Id Required";
                    return BadRequest(response);    
                }

                await _manager.DeleteAsync(id);
                response.StatusCode = 200;
                response.Message = Constants.deleted;
                return BadRequest(response);
            }
            catch(Exception ex)
            {
                return await _exceptionHandleService.HandleException(ex);
            }
        }
    }
}
