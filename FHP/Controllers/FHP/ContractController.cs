﻿using DocumentFormat.OpenXml.ExtendedProperties;
using FHP.infrastructure.DataLayer;
using FHP.infrastructure.Manager.FHP;
using FHP.infrastructure.Service;
using FHP.models.FHP.Contract;
using FHP.utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NAudio.Midi;

namespace FHP.Controllers.FHP
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractController : ControllerBase
    {
        private readonly IContractManager _manager;
        private readonly IExceptionHandleService _exceptionHandleService;
        private readonly IUnitOfWork _unitOfWork;
        public ContractController(IContractManager manager, 
                                  IExceptionHandleService exceptionHandleService,
                                  IUnitOfWork unitOfWork)
        {
            _manager=manager;
            _exceptionHandleService = exceptionHandleService;
            _unitOfWork = unitOfWork;
        }


        // API endpoint to add Contract
        [HttpPost("add")]   
        public async Task<IActionResult> AddAsync(AddContractModel model)
        {
            if (!ModelState.IsValid)
            {
                //it returns a BadRequest response with a list of errors.
                return BadRequest(ModelState.GetErrorList()); 
            }

            var response = new BaseResponseAdd();

            //The method then begins a database transaction to ensure data consistency during  addition.
            await using var transaction = await _unitOfWork.BeginTransactionAsync(); 

            try
            {
                if (model.Id == 0 && model.EmployeeId != 0 && model.JobId != 0 && model.EmployerId != 0
                    && !string.IsNullOrEmpty(model.Description)
                    && !string.IsNullOrEmpty(model.EmployeeSignature)
                    && !string.IsNullOrEmpty(model.EmployerSignature))

                {
                    // Add the contract model asynchronously.
                    await _manager.AddAsync(model);

                    // Commit the transaction.
                    await transaction.CommitAsync();  
                    response.StatusCode = 200;
                    response.Message = Constants.added;
                    return Ok(response);
                }

                // If necessary fields are not provided in the model, return a BadRequest response.
                response.StatusCode = 400;
                response.Message = Constants.provideValues;
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                // In case of any exceptions during the process, roll back the transaction.
                await transaction.RollbackAsync();

                // Handle the exception using the provided exception handling service.
                return await _exceptionHandleService.HandleException(ex);  
            }
        }


        // API endpoint to edit Contract 
        [HttpPut("edit")]  
        public async Task<IActionResult> EditAsync(AddContractModel model)
        {
            if (!ModelState.IsValid)
            {
                //it returns a BadRequest response with a list of errors.
                return BadRequest(ModelState.GetErrorList()); 
            }
           
            // Response object to be sent back.
            var response = new BaseResponseAdd();

            // Begin a database transaction to ensure data consistency during updation.
            await using var transaction = await _unitOfWork.BeginTransactionAsync(); 

            try
            {
                if(model.Id >= 0)
                {
                    // Edit the Contract model asynchronously.
                    await _manager.Edit(model);

                    // Commit the transaction.
                    await transaction.CommitAsync(); 

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
                // In case of any exceptions during the process, roll back the transaction.
                await transaction.RollbackAsync();

                // Handle the exception using the provided exception handling service.
                return await _exceptionHandleService.HandleException(ex); 
            }
        }

        // Get All Contract with Pagination and search filter
        [HttpGet("getall-pagination")] 
        public async Task<IActionResult> GetAllAsync(int page,int pageSize,string? search)
        {
            if (!ModelState.IsValid)
            {
                //it returns a BadRequest response with a list of errors.
                return BadRequest(ModelState.GetErrorList()); 
            } 

            var response = new BaseResponsePagination<object>();

            try
            {

                // Retrieve data from the manager based on pagination parameters.
                var data = await _manager.GetAllAsync(page,pageSize,search);

                // Check if data is retrieved successfully.
                if (data.contract != null)
                {
                    response.StatusCode = 200;
                    response.Data = data.contract;
                    response.TotalCount = data.totalCount;
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


        // Get By Id Contract 
        [HttpGet("getbyid")] 
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                //it returns a BadRequest response with a list of errors.
                return BadRequest(ModelState.GetErrorList()); 
            }

            var response = new BaseResponseAddResponse<object>();

            try
            {
                // Retrieve Contract data by its Id from the manager.
                var data = await _manager.GetByIdAsync(id);

                // Check if data is retrieved successfully.
                if (data != null)
                {
                    response.StatusCode = 200;
                    response.Data = data;
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

        // Delete Contract by Id
        [HttpDelete("delete/{id}")] 
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                //it returns a BadRequest response with a list of errors
                return BadRequest(ModelState.GetErrorList()); 
            }

            var response = new BaseResponseAdd();


            try
            {
                if(id <= 0)
                {
                    // If Id is not provided or invalid, return a BadRequest response.
                    response.StatusCode = 400;
                    response.Message = "Id Required.";
                    return BadRequest(response);
                }
                // Delete Contract asynchronously using the manager.
                await _manager.DeleteAsync(id); 
                response.StatusCode = 200;
                response.Message = Constants.deleted;
                return Ok(response); 

            }
            catch(Exception ex)
            {
                //exceptionHandle service.
                return await _exceptionHandleService.HandleException(ex); 
            }
        }
    }
}
