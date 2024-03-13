using DocumentFormat.OpenXml.ExtendedProperties;
using FHP.infrastructure.DataLayer;
using FHP.infrastructure.Manager.UserManagement;
using FHP.infrastructure.Service;
using FHP.models.UserManagement.State;
using FHP.services;
using FHP.utilities;
using Hangfire.States;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Bcpg.OpenPgp;

namespace FHP.Controllers.UserManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class StateController : ControllerBase
    {
        private readonly IStateManager _manager;
        private readonly IExceptionHandleService _exceptionHandleService;
        private readonly IUnitOfWork _unitOfWork;
        public StateController(IStateManager manager,
                               IExceptionHandleService exceptionHandleService,
                               IUnitOfWork unitOfWork)
        {
            _manager=manager;
            _exceptionHandleService=exceptionHandleService;
            _unitOfWork = unitOfWork;
        }

        // add State
        [HttpPost("add")]
        public async Task<IActionResult> AddAsync(AddStateModel model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList()); //it returns a BadRequest response with a list of errors.
            }

            var response = new BaseResponseAdd();
            await using var transaction = await _unitOfWork.BeginTransactionAsync(); //The method then begins a database transaction to ensure data consistency during  addition.

            try
            {
                if(model.Id ==0 &&  model.CountryId !=0 && !string.IsNullOrEmpty(model.StateName))
                {
                    await _manager.AddAsync(model); //added
                    await transaction.CommitAsync(); // commit transaction
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
                await transaction.RollbackAsync();  //In case of any exceptions during the process, it rolls back the transaction
                return await _exceptionHandleService.HandleException(ex); //exceptionHandler service
            }
        }

        // edit State
        [HttpPut("edit")]
        public async Task<IActionResult> EditAsync(AddStateModel model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());  //it returns a BadRequest response with a list of errors.
            }

            var response = new BaseResponseAdd();
            await using var transaction = await _unitOfWork.BeginTransactionAsync(); //The method then begins a database transaction to ensure data consistency during  updation.

            try
            {
                if(model.Id >=0 && model != null)
                {
                    await _manager.Edit(model); //updated
                    await transaction.CommitAsync(); //commit transaction
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
                return await _exceptionHandleService.HandleException(ex);  //exceptionHandler service
            }
        }

        //get all State
        [HttpGet("getall-pagination")]
        public async Task<IActionResult> GetAllAsync(int page,int pageSize,string? search)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList()); //it returns a BadRequest response with a list of errors.
            }

            var response = new BaseResponsePagination<object>();

            try
            {
                var data = await _manager.GetAllAsync(page,pageSize,search);
                if (data.state != null)
                {
                    response.StatusCode = 200;
                    response.Data = data.state;
                    response.TotalCount = data.totalCount;
                    return Ok(response);
                }

                response.StatusCode = 400;
                response.Message = Constants.error;
                return BadRequest(response);
            }
            catch(Exception ex) 
            { 
                return await _exceptionHandleService.HandleException(ex);    // exception service
            }

        }

        //get by id State
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
                return await _exceptionHandleService.HandleException(ex);    //exceptionhandler service
            }
        }

        // get by CountryId
        [HttpGet("getby-countryId")]
        public async Task<IActionResult> GetByCountryIdAsync(int countryId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList()); //it returns a BadRequest response with a list of errors.
            }

            var response = new BaseResponseAddResponse<object>();

            try
            {
                var data = await _manager.GetByCountryIdAsync(countryId);
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
                return await _exceptionHandleService.HandleException(ex); //exceptionhandler service
            }
        }


        // delete State
        [HttpDelete("delete/{id}")]
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
                    response.Message = "ID Required";
                    return BadRequest(response);
                }

                await _manager.DeleteAsync(id);
                response.StatusCode = 200;
                response.Message = Constants.deleted; // deleted
                return Ok(response);

            }
            catch(Exception ex)
            {
                return await _exceptionHandleService.HandleException(ex); //exceptionhandler service
            }
        }


    }
}
