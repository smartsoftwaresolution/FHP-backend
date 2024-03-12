using FHP.infrastructure.DataLayer;
using FHP.infrastructure.Manager.FHP;
using FHP.infrastructure.Service;
using FHP.models.FHP;
using FHP.utilities;
using Microsoft.AspNetCore.Mvc;

namespace FHP.Controllers.FHP
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeAvailabilityController : ControllerBase
    {
        private readonly IEmployeeAvailabilityManager _manager;
        private readonly IExceptionHandleService _exceptionHandleService;
        private readonly IUnitOfWork _unitOfWork;


        public EmployeeAvailabilityController(IEmployeeAvailabilityManager manager,
             IExceptionHandleService exceptionHandleService,
             IUnitOfWork unitOfWork)
        {
            _manager=manager;
            _exceptionHandleService=exceptionHandleService;
            _unitOfWork=unitOfWork;
        }

        [HttpPost("add")] //Add EmployeeAvailability 
        public async Task<IActionResult> AddAsync(AddEmployeeAvailabilityModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList()); //it returns a BadRequest response with a list of errors.
            }

            var response = new BaseResponseAdd();

            await using var  transaction = await _unitOfWork.BeginTransactionAsync(); //The method then begins a database transaction to ensure data consistency during  addition.

            try
            {
                if(model.Id == 0 && model.UserId != 0 && model.JobId != 0 && model.EmployeeId != 0)
                {
                    await _manager.AddAsync(model); //EmployeeAvaliability Added
                    await transaction.CommitAsync(); //commit transaction
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
                await transaction.RollbackAsync(); //In case of any exceptions during the process, it rolls back the transaction.
                return await _exceptionHandleService.HandleException(ex); //exceptionHandle service
            }
        }


        [HttpPut("edit")] //Edit EmployeeAvailability
        public async Task<IActionResult> EditAsync(AddEmployeeAvailabilityModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList()); //it returns a BadRequest response with a list of errors.
            }

            var response = new BaseResponseAdd();
            await using var transaction = await _unitOfWork.BeginTransactionAsync(); //The method then begins a database transaction to ensure data consistency during  updation.

            try
            {
                if(model.Id >= 0)
                {
                    await _manager.Edit(model); //Updated
                    await transaction.CommitAsync(); // commit transaction
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
                await transaction.RollbackAsync();  //In case of any exceptions during the process, it rolls back the transaction.
                return await _exceptionHandleService.HandleException(ex);    //exceptionHandle service
            }
        }

        [HttpGet("getall-pagination")] // GetAll EmployeeAvalibility Detail
        public async Task<IActionResult> GetAllAsync(int page , int pageSize,string? search)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList()); //it returns a BadRequest response with a list of errors.
            }

             var response = new BaseResponsePagination<object>(); 
            try
            {
                var data = await _manager.GetAllAsync(page,pageSize,search);
                if(data.employeeAval != null)
                {
                    response.StatusCode = 200;
                    response.Data = data.employeeAval;
                    response.TotalCount = data.totalCount;
                    return Ok(response);
                }

                response.StatusCode = 400;
                response.Message = Constants.error;
                return BadRequest(response);
            }
            catch(Exception ex)
            {
                return await _exceptionHandleService.HandleException(ex);  //exceptionHandle service
            }
        }


        [HttpGet("GetByEmployeeId")] //GetByEmployeeId 
        public async Task<IActionResult> GetByEmployeeIdAsync(int employeeId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList()); //it returns a BadRequest response with a list of errors.
            }

            var response = new BaseResponseAddResponse<object>();

            try
            {
                var data = await _manager.GetByEmployeeIdAsync(employeeId); 
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
                return await _exceptionHandleService.HandleException(ex); // error handling service
            }
        }


        [HttpGet("getbyid")]  //GetById 
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
            catch (Exception ex)
            {
                return await _exceptionHandleService.HandleException(ex);  // exceptionhandler service
            }
        }


        [HttpGet("SetEmployeeAvalibility")] // Employee can set his/her availibility for the job
        public async Task<IActionResult> SetEmployeeAvalibiliyAsync(int EmployeeId, int JobId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList()); //it returns a BadRequest response with a list of errors.
            }

            var response = new BaseResponseAdd();

            try
            {
                if(EmployeeId <= 0)
                {
                    response.StatusCode = 400;
                    response.Message = "Id Required!";
                    return BadRequest(response);
                }

                string result = await _manager.SetEmployeeAvalibility(EmployeeId,JobId);
                response.StatusCode = 200;
                response.Message = $"Employee {result} Now!!"; // SetEmployeeAvalibiliy Succesfully!
                return Ok(response);
            }
            catch(Exception ex)
            {
               return await _exceptionHandleService.HandleException(ex); // exceptionhandler service
            }
        }
        [HttpGet("GetAllAvalibility")] //GetAll by EmployeeAvalibility
        public async Task<IActionResult> GetAllAvalibility(int JobId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList()); //it returns a BadRequest response with a list of errors.
            }

            var response = new BaseResponseAddResponse<object>();

            try
            {
                var data = await _manager.GetAllAvalibility(JobId); 
                if (data != null)
                 {
                    response.StatusCode = 200;
                    response.Data=data;
                    return Ok(response);
                 }

                response.StatusCode = 400;
                response.Message = Constants.error;
                return BadRequest(response);

            }

            catch( Exception ex)
            {
                return await _exceptionHandleService.HandleException(ex);  // exceptionhandler service
            }
        }

        [HttpDelete("delete/{id}")] //delete EmployeeAvalibility
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
                    response.Message = " Id Required ";
                    return BadRequest(response);
                }

                await _manager.DeleteAsync(id);
                response.StatusCode = 200;
                response.Message = Constants.deleted;
                return Ok(response);

            }
            catch (Exception ex)
            {
                return await _exceptionHandleService.HandleException(ex); // exceptionhandler service
            }
        }
    }
}
