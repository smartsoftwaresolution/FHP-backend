
using FHP.infrastructure.Manager.FHP;
using FHP.infrastructure.Service;
using FHP.models.FHP;
using FHP.utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;

namespace FHP.Controllers.FHP
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeDetailController : ControllerBase
    {
        private readonly IEmployeeDetailManager _manager;
        private readonly IExceptionHandleService _exceptionHandleService;
        private readonly IFileUploadService _fileUploadService;
        public EmployeeDetailController(IEmployeeDetailManager manager,
                                        IExceptionHandleService exceptionHandleService,
                                        IFileUploadService fileUploadService)
        {
            _manager = manager;
            _exceptionHandleService = exceptionHandleService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddAsync([FromForm]AddEmployeeDetailModel model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }

            var response = new BaseResponseAdd();

            try
            {
                if( model.UserId !=0 && model.CountryId !=0 && model.StateId!=0 && model.CityId !=0
                   && !string.IsNullOrEmpty(model.MaritalStatus)
                   && !string.IsNullOrEmpty(model.Gender)
                   && !string.IsNullOrEmpty(model.PermanentAddress)
                   && !string.IsNullOrEmpty(model.Mobile))
                   
                {
                    string profileImg = string.Empty;
                    string profileResume = string.Empty;
                    if (model.ProfileImgURL != null)
                    {
                      profileImg =  await _fileUploadService.UploadIFormFileAsync(model.ProfileImgURL);
                    }
                    if (model.ResumeURL != null)
                    {
                        profileResume = await _fileUploadService.UploadIFormFileAsync(model.ResumeURL);
                    }
                    await _manager.AddAsync(model,profileResume);
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
        public async Task<IActionResult> EditAsync([FromForm] AddEmployeeDetailModel model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }

            var response = new BaseResponseAdd();

            try
            {
                if(model.Id >= 0)
                {
                    string resumeUrl = string.Empty;
                    if (model.ResumeURL != null)
                    {
                        resumeUrl = await _fileUploadService.UploadIFormFileAsync(model.ResumeURL);
                    }
                    await _manager.Edit(model,resumeUrl);
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
                return await _exceptionHandleService.HandleException(ex);
            }
        }

        [HttpGet("getall-pagination")]
        public async Task<IActionResult> GetAllAsync(int page,int pagesize,int userId,string? search)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }

            var response = new BaseResponsePagination<object>();

            try
            {
                var data = await _manager.GetAllAsync(page,pagesize,userId,search);
                if(data.employee != null)
                {
                    response.StatusCode = 200;
                    response.Data = data.employee;
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
                return Ok(response);
            }
            catch(Exception ex)
            {
                return await _exceptionHandleService.HandleException(ex);
            }
        }

        [HttpPatch("set-availability/{id}")]
        public async Task<IActionResult> SetAvailabilityAsync(int id)
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

                string result = await _manager.SetAvailabilityAsync(id);
                response.StatusCode = 200;
                response.Message = $"Employee {result} Now!!";
                return Ok(response);
            }
            catch (Exception ex)
            {
                return await _exceptionHandleService.HandleException(ex);
            }
        }


    }
}
