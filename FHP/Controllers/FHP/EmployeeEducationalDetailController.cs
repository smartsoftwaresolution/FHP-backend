﻿using FHP.infrastructure.DataLayer;
using FHP.infrastructure.Manager.FHP;
using FHP.infrastructure.Service;
using FHP.models.FHP.EmployeeEducationalDetail;
using FHP.utilities;
using Microsoft.AspNetCore.Mvc;

namespace FHP.Controllers.FHP
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeEducationalDetailController : ControllerBase
    {
        private readonly IEmployeeEducationalDetailManager _manager;
        private readonly IExceptionHandleService _exceptionHandleService;
        private readonly IUnitOfWork _unitOfWork;
        public EmployeeEducationalDetailController(IEmployeeEducationalDetailManager manager,
                                                   IExceptionHandleService exceptionHandleService,
                                                   IUnitOfWork unitOfWork)
        {
            _manager=manager;
            _exceptionHandleService=exceptionHandleService;
            _unitOfWork=unitOfWork;
        }


        [HttpPost("add")] // Add EmployeeEducationalDetail 
        public async Task<IActionResult> AddAsync(AddEmployeeEducationalDetailModel model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList()); //it returns a BadRequest response with a list of errors.
            }

            var response = new BaseResponseAdd();
            await using var transaction = await _unitOfWork.BeginTransactionAsync();  //The method then begins a database transaction to ensure data consistency during  addition.

            try
            {
                // Check if all required fields are provided and not null or empty.
                if (model.Id == 0 && model.UserId != 0 && model.YearOfCompletion != 0 && model.GPA != 0.0 &&
                    !string.IsNullOrEmpty(model.Education) &&
                    !string.IsNullOrEmpty(model.NameOfBoardOrUniversity))

                {
                    // Add the educational detail using the manager service.
                    await _manager.AddAsync(model);
                   
                    //commit transaction
                    await transaction.CommitAsync(); 
                    response.StatusCode = 200;
                    response.Message = Constants.added; 
                    return Ok(response);
                }
                // If required fields are not provided or empty, return a BadRequest response with an error message.
                response.StatusCode = 400;
                response.Message = Constants.provideValues;
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(); //In case of any exceptions during the process, it rolls back the transaction.
                return await _exceptionHandleService.HandleException(ex); 
            }
        }

        [HttpPut("edit")] // Edit EmployeeEducationDetail
        public async Task<IActionResult> EditAsync(AddEmployeeEducationalDetailModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList()); //it returns a BadRequest response with a list of errors.
            }
            await using var transaction = await _unitOfWork.BeginTransactionAsync(); //The method then begins a database transaction to ensure data consistency during  updation.

            var response = new BaseResponseAdd();

            try
            {
                // Check if the provided model has a valid ID.
                if (model.Id >= 0)
                {
                    // Update the educational detail using the manager service.
                    await _manager.Edit(model);

                    //commit transaction
                    await transaction.CommitAsync(); 
                    response.StatusCode = 200;
                    response.Message = Constants.updated;
                    return Ok(response);
                }
               
                // If the ID is invalid, return a BadRequest response with an error message.
                response.StatusCode = 400;
                response.Message = Constants.provideValues;
                return BadRequest(response);
            }

            catch(Exception ex)
            {
                await transaction.RollbackAsync(); //In case of any exceptions during the process, it rolls back the transaction.
                return await _exceptionHandleService.HandleException(ex);
            }
        }

        [HttpGet("getall-pagination")] // Get All EmployeeEducationDetail
        public async Task<IActionResult> GetAllAsync(int page,int pageSize,int userId,string? search)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList()); //it returns a BadRequest response with a list of errors.
            }

            var response = new BaseResponsePagination<object>();

            try
            {
                // Retrieve all employee education details based on the provided parameters.
                var data = await _manager.GetAllAsync(page, pageSize,userId,search);

                // Check if the retrieved data is not null.
                if (data.employeeeducationaldetail != null)
                {
                    response.StatusCode = 200;
                    response.Data = data.employeeeducationaldetail;
                    response.TotalCount = data.totalCount;
                    return Ok(response);
                }

                // If the retrieved data is null, return a BadRequest response with an error message.
                response.StatusCode = 400;
                response.Message = Constants.error;
                return BadRequest(response);
            }

            catch (Exception ex)
            {
                return await _exceptionHandleService.HandleException(ex); //exception service
            }
        }

        [HttpGet("getbyid")]  // get by Id EmployeeEduactionDetail
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList()); //it returns a BadRequest response with a list of errors.
            }

            var response = new BaseResponseAddResponse<object>();

            try
            {
                // Retrieve employee education detail by the provided ID.
                var data = await _manager.GetByIdAsync(id);

                // Check if the retrieved data is not null.
                if (data != null)
                {
                    response.StatusCode = 200;
                    response.Data = data;
                    return Ok(response);
                }
                // If the retrieved data is null, return a BadRequest response with an error message.
                response.StatusCode = 400;
                response.Message = Constants.error;
                return BadRequest(response);
            }
            catch(Exception ex)
            {
              return  await _exceptionHandleService.HandleException(ex);   //exceptionhandler service
            }
        }


        [HttpDelete("delete/{id}")] // delete EmployeeEducationDetail by Id
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList()); //it returns a BadRequest response with a list of errors.
            }

            var response = new BaseResponseAdd();

            try
            {
                // Check if the provided ID is valid.
                if (id <= 0)
                {
                    response.StatusCode = 400;
                    response.Message = "Id Required";
                    return BadRequest(response);
                }
                // Delete the employee education detail by the provided ID.
                await _manager.DeleteAsync(id);
                response.StatusCode = 200;
                response.Message = Constants.deleted;
                return Ok(response);
            }
            catch(Exception ex)
            {
                return await _exceptionHandleService.HandleException(ex); // exceptionhandler service
            }
        }
    }
}
