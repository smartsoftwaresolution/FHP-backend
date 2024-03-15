using FHP.infrastructure.Manager.UserManagement;
using FHP.infrastructure.Service;
using FHP.models.UserManagement.EmailSetting;
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


        //  API Endpoint for add email settings
        [HttpPost("add")] 
        public async Task<IActionResult> AddAsync(AddEmailSettingModel model)
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
                // Validates the required fields for adding email settings
                if (model.Id == 0  &&
                   !string.IsNullOrEmpty(model.Email) &&
                   !string.IsNullOrEmpty(model.Password) &&
                   !string.IsNullOrEmpty(model.AppPassword) &&
                   !string.IsNullOrEmpty(model.IMapHost) &&
                   !string.IsNullOrEmpty(model.IMapPort) &&
                   !string.IsNullOrEmpty(model.SmtpHost)&&
                   !string.IsNullOrEmpty(model.SmtpPort))                
                {
                    // Calls the manager to add email settings asynchronously
                    await _manager.AddAsync(model); 

                    response.StatusCode = 200;
                    response.Message = Constants.added;

                    // Returns Ok response with success message
                    return Ok(response);
                }


                response.StatusCode = 400;
                response.Message = Constants.provideValues;
                return BadRequest(response);
            }
            catch(Exception ex)
            {
                // Handle the exception using the provided exception handling service.
                return await _exceptionHandleService.HandleException(ex); 
            }
        }


        //  API Endpoint for edit email settings
        [HttpPut("edit")]
        public async Task<IActionResult> EditAsync(AddEmailSettingModel model)
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
                // Checks if the model ID is greater than or equal to 0
                if (model.Id>=0 && model != null)
                {

                    // Calls the manager to edit email settings asynchronously
                    await _manager.EditAsync(model); 
                    response.StatusCode = 200;
                    response.Message = Constants.updated;

                    // Returns Ok response with the success message
                    return Ok(response);
                }

                response.StatusCode = 400;
                response.Message = Constants.provideValues;
                return BadRequest(response);
            }
            catch(Exception ex)
            {
                // Handle the exception using the provided exception handling service.
                return await _exceptionHandleService.HandleException(ex); 
            }
        }


        // API Endpoint for retrieving all emailsetting with pagination and sorting
        [HttpGet("getall-pagination")] 
        public async Task<IActionResult> GetAllAsync(int page,int pageSize,string? search)
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
                // Calls the manager to retrieve email setting asynchronously with pagination and search
                var data =await _manager.GetAllAsync(page,pageSize,search);

                // Checks if the retrieved data is not null
                if (data.emailSetting != null)
                {
                    response.StatusCode = 200;
                    response.Data = data.emailSetting;
                    response.TotalCount = data.totalCount;

                    // Returns Ok response with the data
                    return Ok(response);
                }

                response.StatusCode = 400;
                response.Message = Constants.error;
                return BadRequest(response);
            }
            catch(Exception ex)
            {
                // Handle the exception using the provided exception handling service.
                return await _exceptionHandleService.HandleException(ex);    
            }
        }


        // API Endpoint for retrieving a emailsetting by its ID
        [HttpGet("getbyid")] 
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            // Checks if the model state is valid
            if (!ModelState.IsValid)
            {
                // Returns a BadRequest response with a list of errors if model model state is not valid
                return BadRequest(ModelState.GetErrorList()); 
            }

            var response = new BaseResponseAddResponse<object>();

            try
            {
                // Calls the manager to retrieve a emailsetting by its ID asynchronously
                var data = await _manager.GetByIdAsync(id);


                // Checks if the retrieved data is not null
                if (data != null)
                {
                    // Sets StatusCode to 200 indicating success
                    response.StatusCode = 200;
                    response.Data = data;
                    return Ok(response);
                }

                response.StatusCode = 400;
                response.Message = Constants.error;

                // Returns BadRequest response with the error message
                return BadRequest(response);
            }
            catch(Exception ex)
            {
                // Handle the exception using the provided exception handling service.
                return await _exceptionHandleService.HandleException(ex); 
            }
        }

      
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
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
                // Checks if the provided ID is less than or equal to 0
                if (id <= 0)
                {
                    response.StatusCode = 400;
                    response.Message = "ID Required";


                    // Returns BadRequest response with the error message
                    return BadRequest(response);
                }

                // Calls the manager to delete a emailsetting by its ID asynchronously
                await _manager.DeleteAsync(id);
                response.StatusCode = 200;
                response.Message = Constants.deleted;

                // Returns Ok response with the success message
                return Ok(response);
            }
            catch(Exception ex)
            {
                // Handle the exception using the provided exception handling service.
                return await _exceptionHandleService.HandleException(ex); 
            }
        }
    }
}
