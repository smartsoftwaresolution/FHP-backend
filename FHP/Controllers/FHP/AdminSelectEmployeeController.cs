using FHP.infrastructure.DataLayer;
using FHP.infrastructure.Manager.FHP;
using FHP.infrastructure.Service;
using FHP.models.FHP.AdminSelectEmployee;
using FHP.utilities;
using Microsoft.AspNetCore.Mvc;

namespace FHP.Controllers.FHP
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminSelectEmployeeController : ControllerBase
    {
        private readonly IAdminSelectEmployeeManager _manager;
        private readonly IExceptionHandleService _exceptionHandleService;
        private readonly IUnitOfWork _unitOfWork;
        public AdminSelectEmployeeController(IAdminSelectEmployeeManager manager,
                                             IExceptionHandleService exceptionHandleService,
                                             IUnitOfWork unitOfWork)
        {
            _manager = manager;
            _exceptionHandleService= exceptionHandleService;
            _unitOfWork= unitOfWork;
        }


        [HttpPost("add")] //   Add AdminSelectEmployee 
        public async Task<IActionResult> AddAsync(AddAdminSelectEmployeeModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList()); //it returns a BadRequest response with a list of errors.
            }

            await using var transaction = await _unitOfWork.BeginTransactionAsync();  //The method then begins a database transaction to ensure data consistency during  addition.
            var response = new BaseResponseAdd();

            try
            {
                if(model.Id == 0 && model.JobId != 0 && model.EmployeeId != 0)
                {
                    await _manager.AddAsync(model);
                    await transaction.CommitAsync();  //commit the transaction.
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

                return await _exceptionHandleService.HandleException(ex);  //exception handle service  
            }
        }

        [HttpPut("edit")] // Edit AdminSelectEmployee
        public async Task<IActionResult> EditAsync(AddAdminSelectEmployeeModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList()); //it returns a BadRequest response with a list of errors.
            }
            await using var transaction = await _unitOfWork.BeginTransactionAsync(); //The method then begins a database transaction to ensure data consistency during  updation.

            var response = new BaseResponseAdd();

            try
            {
                if(model.Id >= 0)
                {
                    await _manager.Edit(model); 
                    await transaction.CommitAsync(); //commit the transaction
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
                return await _exceptionHandleService.HandleException(ex);    //exception handle service  
            }
        }


        [HttpGet("getall-pagination")] // Get All AdminSelectEmpoyeeDetail
        public async Task<IActionResult> GetAllAsync(int page,int pageSize,int jobId,string? search)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList()); //it returns a BadRequest response with a list of errors.
            }

            var response = new BaseResponsePagination<object>();

            try
            {
                var data = await _manager.GetAllAsync(page,pageSize,jobId, search);

                if (data.adminSelect != null)
                {
                    response.StatusCode = 200;
                    response.Data = data.adminSelect;
                    response.TotalCount = data.totalCount;
                    return Ok(response);
                }

                response.StatusCode = 400;
                response.Message = Constants.error;
                return BadRequest(response);

            }

            catch (Exception ex)
            {
                return await _exceptionHandleService.HandleException(ex); //exception handle service 
            }
        }



        [HttpGet("getbyid")] //Get By Id AdminSelectEmployee
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            if (!ModelState.IsValid)
            { 
                return BadRequest(ModelState.GetErrorList());    //it returns a BadRequest response with a list of errors.
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
                return await _exceptionHandleService.HandleException(ex); //exception handle service
            }
              
        }

        [HttpDelete("delete/{id}")] //Delete AdminSelectEmployee
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (!ModelState.IsValid)
            { 
                return BadRequest(ModelState.GetErrorList());  //it returns a BadRequest response with a list of errors.
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
                response.StatusCode=200;
                response.Message = Constants.deleted;
                return Ok(response);

            }
            catch(Exception ex)
            { 
                return await _exceptionHandleService.HandleException(ex);  //exception handle service
            }
        }


        [HttpGet("getall-job-employee")] //Get All JobEmployee Details
        public async Task<IActionResult> GetAllJobEmployeeAsync(int jobId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList()); //it returns a BadRequest response with a list of errors.
            }

            var response = new BaseResponsePagination<object>();

            try
            {
                var data = await _manager.GetAllJobEmployeeAsync(jobId);

                if (data.adminSelect != null)
                {
                    response.StatusCode = 200;
                    response.Data = data.adminSelect;
                    response.TotalCount = data.totalCount;
                    return Ok(response);
                }

                response.StatusCode = 400;
                response.Message = Constants.error;
                return BadRequest(response);

            }

            catch (Exception ex)
            { 
                return await _exceptionHandleService.HandleException(ex); //exception handle service
            }
        }

    }
}