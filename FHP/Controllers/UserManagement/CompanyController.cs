using FHP.infrastructure.Manager.UserManagement;
using Microsoft.AspNetCore.Mvc;
using FHP.utilities;
using FHP.infrastructure.Service;
using FHP.models.UserManagement.Company;

namespace FHP.Controllers.UserManagement
{

    [CustomAuthorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {

        private readonly ICompanyManager _manager;
        private readonly IExceptionHandleService _exceptionHandleService;
        public CompanyController(ICompanyManager manager, IExceptionHandleService exceptionHandleService)
        {
            _manager=manager;
            _exceptionHandleService=exceptionHandleService;
        }
       

        [HttpPost("add")]
        public async Task<IActionResult> AddAsync(AddCompanyModel model)
        {
            if (!ModelState.IsValid)
            {
                return Ok(ModelState.GetErrorList());
            }

            var response = new BaseResponseAdd();

            try
            {
                if (model.Id == 0 && model.UserId != 0 && 
                    !string.IsNullOrEmpty(model.Name)  && 
                    !string.IsNullOrEmpty(model.Description))
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
        public async Task<IActionResult> Edit(AddCompanyModel model)
        {
            if (!ModelState.IsValid)
            {
                return Ok(ModelState.GetErrorList());
            }
            var response = new BaseResponseAdd();

            try
            {
                if(model.Id>0 && model !=null)
                {   
                    await _manager.EditAsync(model);
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


        [HttpGet("getall")]
        public async Task<IActionResult> GetAllAsync(int userId)
        {
            if (!ModelState.IsValid)
            {
                return Ok(ModelState.GetErrorList());
            }

            var response = new BaseResponseAddResponse<object>();

            try
            {
                var data = await _manager.GetAllAsync(userId);

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
                return await _exceptionHandleService.HandleException(ex);
            }
        }


        [HttpGet("getbyid")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Ok(ModelState.GetErrorList());
            }

            var response = new BaseResponseAddResponse<object>();

            try
            {
                var data =await _manager.GetByIdAsync(id);

                if (data != null)
                {
                    response.StatusCode = 200;
                    response.Data = data;
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


        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Ok(ModelState.GetErrorList());
            }
           
            var response = new BaseResponseAdd();
           
            try
            {
               
                if (id <= 0)
                {
                    response.Message = "Id required";
                    response.StatusCode = 400;
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
