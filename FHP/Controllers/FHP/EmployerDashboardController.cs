using FHP.infrastructure.Manager.FHP;
using FHP.infrastructure.Service;
using FHP.utilities;
using Microsoft.AspNetCore.Mvc;

namespace FHP.Controllers.FHP
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployerDashboardController : ControllerBase
    {
        private readonly IEmployerDashboardManager _manager;
        private readonly IExceptionHandleService _exceptionHandleService;
        public EmployerDashboardController(IEmployerDashboardManager manager,
                                           IExceptionHandleService exceptionHandleService)
        {
            _manager = manager;
            _exceptionHandleService = exceptionHandleService;
        }
                                                        
        [HttpGet("getAll-jobPosts")]
        public async Task<IActionResult> JobPosts()
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }

            var response = new BaseResponseCount();

            try
            {
                var data = await _manager.GetAllJobPost();

                if(data != null)
                {
                    response.StatusCode = 200;
                    response.JobPost = data;
                    return Ok(response);
                }  

                response.StatusCode = 404;
                response.Message = "Not found";
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                return await _exceptionHandleService.HandleException(ex);
            }
        }

        [HttpGet("getAll-jobPostDraft")]
        public async Task<IActionResult> JobPostDraft()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }

            var response = new BaseResponseCount();

            try
            {
                var data = await _manager.GetAllDraftPost();

                if (data != null)
                {
                    response.StatusCode = 200;
                    response.DraftPost = data;
                    return Ok(response);
                }

                response.StatusCode = 404;
                response.Message = "Not found";
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                return await _exceptionHandleService.HandleException(ex);
            }
        }

        [HttpGet("getAll-contract")]
        public async Task<IActionResult> GetContract(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }

            var response = new BaseResponseCount();

            try
            {
                var data = await _manager.GetAllContract(id);

                if (data != null)
                {
                    response.StatusCode = 200;
                    response.TotalContract = data;
                    return Ok(response);
                }

                response.StatusCode = 404;
                response.Message = "Not found";
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                return await _exceptionHandleService.HandleException(ex);
            }
        }

        [HttpGet("getAll-jobRequest")]
        public async Task<IActionResult> TotalJobRequest(int employeeId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }

            var response = new BaseResponseCount();

            try
            {
               var data = await _manager.TotalJobReq(employeeId);
                if(data != null)
                {
                    response.StatusCode = 200;
                    response.TotalJobRequest = data;
                    return Ok(response);
                }

                response.StatusCode = 404;
                response.Message = "Not found";
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                return await _exceptionHandleService.HandleException(ex);
            }
        }
    }
}
