using FHP.infrastructure.Manager.UserManagement;
using FHP.infrastructure.Service;
using FHP.models.UserManagement;
using FHP.utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FHP.Controllers.UserManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailSettingController : ControllerBase
    {
        private readonly IEmailSettingManager _manager;
        private readonly IExceptionHandleService _exceptionHandleService;
        public EmailSettingController(IEmailSettingManager manager, IExceptionHandleService exceptionHandleService)
        {
            _manager = manager;
            _exceptionHandleService = exceptionHandleService;
        }


        //add Email Setting
        [HttpPost("add")]
        public async Task<IActionResult> AddAsync(AddEmailSettingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList()); //it returns a BadRequest response with a list of errors.
            }

            var response = new BaseResponseAdd();

            try
            {
                
                if(model.Id == 0  &&
                   !string.IsNullOrEmpty(model.Email) &&
                   !string.IsNullOrEmpty(model.Password) &&
                   !string.IsNullOrEmpty(model.AppPassword) &&
                   !string.IsNullOrEmpty(model.IMapHost) &&
                   !string.IsNullOrEmpty(model.IMapPort) &&
                   !string.IsNullOrEmpty(model.SmtpHost)&&
                   !string.IsNullOrEmpty(model.SmtpPort))                
                {
                    await _manager.AddAsync(model); // added
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
                return await _exceptionHandleService.HandleException(ex); // exceptionHandler service
            }
        }


        // edit emailsetting
        [HttpPut("edit")]
        public async Task<IActionResult> EditAsync(AddEmailSettingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList()); //it returns a BadRequest response with a list of errors.
            }
            
            var response = new BaseResponseAdd();

            try
            {
               
                if(model.Id>=0 && model != null)
                {
                    await _manager.EditAsync(model); // updated
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
                return await _exceptionHandleService.HandleException(ex); // exceptionHandler service
            }
        }


        // get all emialsetting
        [HttpGet("getall-pagination")]
        public async Task<IActionResult> GetAllAsync(int page,int pageSize,string? search)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList()); //it returns a BadRequest response with a list of errors.
            }

            var response = new BaseResponsePagination<object>();

            try
            {
               
                var data=await _manager.GetAllAsync(page,pageSize,search);
                if (data.emailSetting != null)
                {
                    response.StatusCode = 200;
                    response.Data = data.emailSetting;
                    response.TotalCount = data.totalCount;
                    return Ok(response);
                }

                response.StatusCode = 400;
                response.Message = Constants.error;
                return BadRequest(response);
            }
            catch(Exception ex)
            {
                return await _exceptionHandleService.HandleException(ex);    // exceptionHandler service
            }
        }


        //get by id emailsetting
        [HttpGet("getbyid")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList()); //it returns a BadRequest response with a list of errors.
            }

            var response = new BaseResponseAddResponse<object>();

            try
            {
                
                var data = await _manager.GetByIdAsync(id);

                if (data != null)
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

                return await _exceptionHandleService.HandleException(ex); // exceptionHandler service
            }
        }

        // delete emailsetting
        [HttpDelete("delete/{id}")]
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
                    response.Message = "ID Required";
                    return BadRequest(response);
                }
                await _manager.DeleteAsync(id);
                response.StatusCode = 200;
                response.Message = Constants.deleted;
                return Ok(response);
            }
            catch(Exception ex)
            {
                return await _exceptionHandleService.HandleException(ex); //exceptionHandler service
            }
        }
    }
}
