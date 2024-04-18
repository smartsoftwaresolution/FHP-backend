using FHP.infrastructure.Manager.FHP;
using FHP.infrastructure.Service;
using FHP.utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FHP.Controllers.FHP
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportManager _reportManager;
        private readonly IExceptionHandleService _exceptionHandleService;

        public ReportController(IReportManager reportManager,
                                IExceptionHandleService exceptionHandleService)
        {
            _reportManager = reportManager;
        }

        [HttpGet("GetAllEmployee")]
        public async Task<IActionResult> GetAllEmployeeAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }

            var response = new BaseResponseAddResponse<object>();

            try
            {
             var data = await _reportManager.GetAllEmployeeAsync(id);

                if(data != null)
                {
                    response.StatusCode = 200;
                    response.Data = data;
                    return Ok(response);
                }

                response.StatusCode = 400;
                response.Message = "Error Occured";
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                return await _exceptionHandleService.HandleException(ex);
            }
        }

       
    }
}
