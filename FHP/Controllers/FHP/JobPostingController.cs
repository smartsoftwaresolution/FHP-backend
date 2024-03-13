using FHP.infrastructure.DataLayer;
using FHP.infrastructure.Manager.FHP;
using FHP.infrastructure.Service;
using FHP.models.FHP.JobPosting;
using FHP.utilities;
using Microsoft.AspNetCore.Mvc;

namespace FHP.Controllers.FHP
{
    [Route("api/[controller]")]
    [ApiController]

    public class JobPostingController : ControllerBase
    {
        private readonly IJobPostingManager _manager;
        private readonly IExceptionHandleService _exceptionHandleService;
        private readonly IUnitOfWork _unitOfWork;
        public JobPostingController(IJobPostingManager manager,
                                    IExceptionHandleService exceptionHandleService,
                                    IUnitOfWork unitOfWork)
        {
            _manager=manager;
            _exceptionHandleService=exceptionHandleService;
            _unitOfWork=unitOfWork;
        }

        //Add Job Posting
        [HttpPost("add")]
        public async Task<IActionResult> AddAsync(AddJobPostingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList()); //it returns a BadRequest response with a list of errors.

            }

            var response = new BaseResponseAdd();
             
            await using var transaction = await _unitOfWork.BeginTransactionAsync(); //The method then begins a database transaction to ensure data consistency during  addition.

            try
            {
                if (model.Id == 0 && model.UserId != 0
                    && !string.IsNullOrEmpty(model.JobTitle)
                    && !string.IsNullOrEmpty(model.Description)
                    && !string.IsNullOrEmpty(model.Experience)
                    && !string.IsNullOrEmpty(model.RolesAndResponsibilities)
                    && !string.IsNullOrEmpty(model.Skills)
                    && !string.IsNullOrEmpty(model.Address)
                    && !string.IsNullOrEmpty(model.Payout))
                    
                {
                    await _manager.AddAsync(model); // added
                    await transaction.CommitAsync();// commit transaction
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
                await transaction.RollbackAsync(); //In case of any exceptions during the process, it rolls back the transaction.
                return await _exceptionHandleService.HandleException(ex); //exception hadler service
            }
        }


        //Edit JobPosting 
        [HttpPut("edit")]
        public async Task<IActionResult> EditAsync(AddJobPostingModel model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList()); //it returns a BadRequest response with a list of errors.
            }

            var response = new BaseResponseAdd();
            await using var transaction = await _unitOfWork.BeginTransactionAsync();  //The method then begins a database transaction to ensure data consistency during  updation.

            try
            {
                if(model.Id >= 0)
                {
                    string message = await _manager.Edit(model); //updated
                    await transaction.CommitAsync(); // commit transaction

                    if (message.Contains("updated successfully"))
                    {


                        response.StatusCode = 200;
                        response.Message = message;
                        return Ok(response);
                    }
                    else
                    {
                        response.StatusCode = 400;
                        response.Message = message;
                        return BadRequest(response);
                    }
                }

                response.StatusCode = 400;
                response.Message = Constants.provideValues;
                return BadRequest(response);
            }
            catch(Exception ex)
            {
                await transaction.RollbackAsync(); //In case of any exceptions during the process, it rolls back the transaction.
                return await  _exceptionHandleService.HandleException(ex); // exceptionHandler service
            } 
        }


        // get all JobPosting
        [HttpGet("getall-pagination")]
        public async Task<IActionResult> GetAllAsync(int page,int pageSize,string? search,int userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList()); //it returns a BadRequest response with a list of errors.
            }

            var response = new BaseResponsePagination<object>();

            try
            {
                var data = await _manager.GetAllAsync(page,pageSize,search,userId);
                if (data.jobPosting != null)
                {
                    response.StatusCode = 200;
                    response.Data = data.jobPosting;
                    response.TotalCount = data.totalCount;
                    return Ok(response);
                }

                response.StatusCode = 400;
                response.Message = Constants.error;
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                return await _exceptionHandleService.HandleException(ex); //exceptionHandler service
            }
        }


        //get by id JobPosting
        [HttpGet("getbyid")]
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
            catch(Exception ex)
            {
                return await _exceptionHandleService.HandleException(ex); //exceptionHandler service
            }
        }


        // delete JobPosting
        [HttpDelete("delete/{id}")]
        public  async Task<IActionResult> DeleteAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());   //it returns a BadRequest response with a list of errors.
            }

            var response = new BaseResponseAdd();

            try
            {
                if(id <= 0)
                {
                    response.StatusCode = 400;
                    response.Message = "Id Required.";
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


        // Activate - decativate JobPosting
        [HttpPatch("active-deactive/{id}")]
        public async Task<IActionResult> ActiveDeactiveAsync(int id)
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
                    response.Message = "Id Required.";
                    return BadRequest(response);
                }

                string result =  await _manager.ActiveDeactiveAsync(id);
                response.StatusCode = 200;
                response.Message = $"Job {result} Successfully!!!";
                return Ok(response);

            }
            catch (Exception ex)
            {
                return await _exceptionHandleService.HandleException(ex); // exceptionHandler service
            }
        }


        // Submit JobPosting
        [HttpPatch("submit-job/{id}")]
        public async Task<IActionResult> SubmitJobAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());  //it returns a BadRequest response with a list of errors.
            }

            var response = new BaseResponseAdd();

            try
            {
                if (id <= 0)
                {
                    response.StatusCode = 400;
                    response.Message = "Id Required.";
                    return BadRequest(response);
                }

                await _manager.SubmitJobAsync(id); //submitted
                response.StatusCode = 200;
                response.Message = $"Job Submitted Successfully!!!";
                return Ok(response);

            }
            catch (Exception ex)
            {
                return await _exceptionHandleService.HandleException(ex); // exceptionHandler service
            }
        }



        // Cancel jobposting
        [HttpPatch("cancel-job/{id}")]
        public async Task<IActionResult> CancelJobAsync(int id,string cancelReason)
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
                    response.Message = "Id Required.";
                    return BadRequest(response);
                }

                await _manager.CancelJobAsync(id,cancelReason); //cancel
                response.StatusCode = 200;
                response.Message = $"Job Cancel Successfully!!!";
                return Ok(response);

            }
            catch (Exception ex)
            {
                return await _exceptionHandleService.HandleException(ex); // exceptionHandler service
            }
        }



        // Setting JobPosting Status

        [HttpPatch("set-job-processing-status/{id}")]
        public async Task<IActionResult> SetJobProcessingStatusAsync(int id, Constants.JobProcessingStatus jobProcessingStatus)
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
                    response.Message = "Id Required.";
                    return BadRequest(response);
                }
                await _manager.SetJobProcessingStatus(id, jobProcessingStatus);
                response.StatusCode = 200;
                response.Message = $"Job Processing Status set Successfully!!!";
                return Ok(response);
            }
            catch (Exception ex)
            {
                return await _exceptionHandleService.HandleException(ex); //exceptionHandler service
            } 
        }
    }
}
