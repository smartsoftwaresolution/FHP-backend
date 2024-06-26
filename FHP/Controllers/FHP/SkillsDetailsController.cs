﻿using FHP.infrastructure.DataLayer;
using FHP.infrastructure.Manager.FHP;
using FHP.infrastructure.Service;
using FHP.models.FHP.SkillDetail;
using FHP.utilities;
using Microsoft.AspNetCore.Mvc;

namespace FHP.Controllers.FHP
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillsDetailsController : ControllerBase
    {
        private readonly ISkillsDetailManager _manager;
        private readonly IExceptionHandleService _exceptionHandleService;
        private readonly IUnitOfWork _unitOfWork;
        public SkillsDetailsController(ISkillsDetailManager manager,
                                       IExceptionHandleService exceptionHandleService,
                                       IUnitOfWork unitOfWork)
        {
            _manager = manager;
            _exceptionHandleService = exceptionHandleService;
            _unitOfWork = unitOfWork;
        }


        //Add SkillsDetail
        [HttpPost("add")]
        public async Task<IActionResult> AddAsync(AddSkillsDetailModel model)
        {
            if (!ModelState.IsValid)
            {
                // If the model state is not valid, return a BadRequest response with a list of errors
                return BadRequest(ModelState.GetErrorList()); 
            }

            var response = new BaseResponseAdd();

            //The method then begins a database transaction to ensure data consistency during  addition.
            await using var transaction = await _unitOfWork.BeginTransactionAsync(); 

            try
            {
                if (model.Id == 0 && model.UserId != 0 && !string.IsNullOrEmpty(model.SkillName))
                {
                    // Add the SkillDetail model asynchronously
                    await _manager.AddAsync(model);

                    // commit transaction
                    await transaction.CommitAsync();

                    // Set response status code and message for successful addition.
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
                //In case of any exceptions during the process, it rolls back the transaction.
                await transaction.RollbackAsync();

                // Handle the exception using the provided exception handling service.
                return await _exceptionHandleService.HandleException(ex);
            }
        }


        // API endpoint to edit SkillDetail
        [HttpPut("edit")] 
        public async Task<IActionResult> EditAsync(AddSkillsDetailModel model)
        {
            if (!ModelState.IsValid)
            {
                // If the model state is not valid, return a BadRequest response with a list of errors.
                return BadRequest(ModelState.GetErrorList()); 
            }

            var response = new BaseResponseAdd();

            try
            {
                if (model.Id >= 0)
                {
                    // Edit the SkillDetail model asynchronously
                    await _manager.Edit(model); 

                    response.StatusCode = 200;
                    response.Message = Constants.updated;

                    // Return OK response with the success message.
                    return Ok(response);
                }

                // If necessary fields are not provided in the model, return a BadRequest response.
                response.StatusCode = 400;
                response.Message = Constants.provideValues;
                return BadRequest(response);

            }
            catch (Exception ex)
            {
                // Handle the exception using the provided exception handling service.
                return await _exceptionHandleService.HandleException(ex); 
            }
        }

        
        [HttpGet("getall-pagination")]
        public async Task<IActionResult> GetAllAsync(int page,int pageSize,string? search)
        {
            if (!ModelState.IsValid)
            {
                // If the model state is not valid, return a BadRequest response with a list of errors.
                return BadRequest(ModelState.GetErrorList()); 
            }

            var response = new BaseResponsePagination<object>();

            try
            {
                // Retrieve data from the manager based on pagination parameters.
                var data = await _manager.GetAllAsync(page, pageSize,search);

                // Check if data is retrieved successfully.
                if (data.skill != null)
                {
                    response.StatusCode = 200;
                    response.Data = data.skill;
                    response.TotalCount = data.totalCount;

                    // Return OK response with the retrieved data.
                    return Ok(response);
                }

                // If data retrieval fails, return a BadRequest response.
                response.StatusCode = 400;
                response.Message = Constants.error;
                return BadRequest(response);

            }
            catch(Exception ex)
            {
                // Handle any exceptions using the provided exception handling service.
                return await _exceptionHandleService.HandleException(ex);
            } 
        }


       
        [HttpGet("getbyid")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                // Handle any exceptions using the provided exception handling service.
                return BadRequest(ModelState.GetErrorList()); 
            }

            var response = new BaseResponseAddResponse<object>();

            try
            {
             //   Retrieves SkillDetail data asynchronously by the provided ID
                var data = await _manager.GetByIdAsync(id);

                // Checks if data is found
                if (data != null)
                {
                    // Sets StatusCode to 200 indicating success
                    response.StatusCode = 200;
                    response.Data = data;

                    // Returns Ok response with the data
                    return Ok(response);
                }
                // If data retrieval fails, return a BadRequest response.
                response.StatusCode = 400;
                response.Message = Constants.error;
                return BadRequest(response);

            }
            catch (Exception ex)
            {
                // Handle any exceptions using the provided exception handling service.
                return await _exceptionHandleService.HandleException(ex); 
            }
        }


       
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                // Returns a BadRequest response with a list of errors if model state is not valid
                return BadRequest(ModelState.GetErrorList()); 
            }

            // Initializes the response object for returning the result
            var response = new BaseResponseAdd();

            try
            {
                // Checks if the provided ID is valid
                if (id <= 0)
                {
                    // Sets StatusCode to 400 indicating a bad request
                    response.StatusCode = 400;
                    response.Message = " Id Required ";
                    return BadRequest(response);
                }

                // Calls the manager to asynchronously delete the skilldetail by ID
                await _manager.DeleteAsync(id);
                response.StatusCode=200;
                response.Message = Constants.deleted;

                // Returns Ok response with the success message
                return Ok(response);

            }
            catch(Exception ex)
            {
                // Handle any exceptions using the provided exception handling service.
                return await _exceptionHandleService.HandleException(ex); 
            }
        }

        [HttpDelete("delete-by-ids")]
        public async Task<IActionResult> DeleteByIdsAsync([FromQuery] List<int> ids)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }

            var response = new BaseResponseAdd();

            try
            {
                if (ids == null)
                {
                    response.StatusCode = 400;
                    response.Message = "Ids Required";
                    return BadRequest(response);
                }

                await _manager.DeleteByAllIdAsync(ids);
                response.StatusCode = 200;
                response.Message = Constants.deleted;
                return Ok(response);
            }
            catch (Exception ex)
            {
                return await _exceptionHandleService.HandleException(ex);
            }
        }
    }
}
