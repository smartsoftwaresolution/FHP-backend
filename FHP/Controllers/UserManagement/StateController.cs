using FHP.infrastructure.DataLayer;
using FHP.infrastructure.Manager.UserManagement;
using FHP.infrastructure.Service;
using FHP.models.UserManagement.State;
using FHP.utilities;
using Microsoft.AspNetCore.Mvc;

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

       
        [HttpPost("add")] // API Endpoint for adding a State
        public async Task<IActionResult> AddAsync(AddStateModel model)
        {
            // Checks if the model state is valid
            if (!ModelState.IsValid)
            {
                //it returns a BadRequest response with a list of errors.
                return BadRequest(ModelState.GetErrorList()); 
            }

            // Initializes the response object for returning the result
            var response = new BaseResponseAdd();

            //The method then begins a database transaction to ensure data consistency during  addition.
            await using var transaction = await _unitOfWork.BeginTransactionAsync(); 

            try
            {
                if(model.Id ==0 &&  model.CountryId !=0 && !string.IsNullOrEmpty(model.StateName))
                {
                    await _manager.AddAsync(model);

                    // commit transaction
                    await transaction.CommitAsync();

                    // Sets StatusCode to 200 indicating success
                    response.StatusCode = 200;
                    response.Message = Constants.added;

                    // Returns Ok response with the success message
                    return Ok(response);

                }

                // Sets StatusCode to 400 indicating a bad request
                response.StatusCode = 400;
                response.Message = Constants.provideValues;

                // Returns BadRequest response with the error message
                return BadRequest(response);
            }
            catch(Exception ex)
            {
                //In case of any exceptions during the process, it rolls back the transaction
                await transaction.RollbackAsync();

                // Handle the exception using the provided exception handling service.
                return await _exceptionHandleService.HandleException(ex); 
            }
        }

        
        [HttpPut("edit")] // API Endpoint for updating a State
        public async Task<IActionResult> EditAsync(AddStateModel model)
        {
            // Checks if the model state is valid
            if (!ModelState.IsValid)
            {
                //it returns a BadRequest response with a list of errors.
                return BadRequest(ModelState.GetErrorList());  
            }

            // Initializes the response object for returning the result
            var response = new BaseResponseAdd();

            //The method then begins a database transaction to ensure data consistency during  updation.
            await using var transaction = await _unitOfWork.BeginTransactionAsync(); 

            try
            {
                if(model.Id >=0 && model != null)
                {
                    await _manager.Edit(model);

                    //commit transaction
                    await transaction.CommitAsync();

                    // Sets StatusCode to 200 indicating success
                    response.StatusCode = 200;
                    response.Message = Constants.updated;

                    // Returns Ok response with the success message
                    return Ok(response);
                }


                // Sets StatusCode to 400 indicating a bad request
                response.StatusCode = 400;
                response.Message = Constants.provideValues;

                // Handle the exception using the provided exception handling service.
                return BadRequest(response);

            }
            catch(Exception ex)
            {
                //In case of any exceptions during the process, it rolls back the transaction.
                await transaction.RollbackAsync();

                // Handle the exception using the provided exception handling service.
                return await _exceptionHandleService.HandleException(ex); 
            }
        }

        
        [HttpGet("getall-pagination")] // API  Endpoint for retrieving all entities with pagination
        public async Task<IActionResult> GetAllAsync(int page,int pageSize,string? search)
        {
            // Checks if the model state is valid
            if (!ModelState.IsValid)
            {
               // Returns a BadRequest response with a list of errors if model state is not valid
                return BadRequest(ModelState.GetErrorList());
            }

            // Initializes the response object for returning the result
            var response = new BaseResponsePagination<object>();

            try
            {
                // Calls the manager to retrieve all entities with pagination and search asynchronously
                var data = await _manager.GetAllAsync(page,pageSize,search);

                // Checks if the retrieved data is not null
                if (data.state != null)
                {
                    // Sets StatusCode to 200 indicating success
                    response.StatusCode = 200;
                    response.Data = data.state;
                    response.TotalCount = data.totalCount;

                    // Returns Ok response with the data
                    return Ok(response);
                }

                // Sets StatusCode to 400 indicating a bad request
                response.StatusCode = 400;
                response.Message = Constants.error;

                // Returns BadRequest response with the error message
                return BadRequest(response);
            }
            catch(Exception ex) 
            {
                // Handle the exception using the provided exception handling service.
                return await _exceptionHandleService.HandleException(ex);    
            }

        }

       
        [HttpGet("getbyid")] // API Endpoint for retrieving an entity by its ID
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            // Checks if the model state is valid
            if (!ModelState.IsValid)
            {
                // Returns a BadRequest response with a list of errors if model state is not valid
                return BadRequest(ModelState.GetErrorList()); 
            }

            // Initializes the response object for returning the result
            var response = new BaseResponseAddResponse<object>();

            try
            {
                // Calls the manager to retrieve an entity by its ID asynchronously
                var data = await _manager.GetByIdAsync(id);

                // Checks if the retrieved data is not null
                if (data != null)
                {
                    response.StatusCode = 200;
                    response.Data = data;

                    // Returns Ok response with the data
                    return Ok(response);
                }

                response.StatusCode = 400;
                response.Message = Constants.error;

                // Returns BadRequest response with the error message
                return BadRequest(response);
            }
            catch(Exception ex)
            {
                // Handle the exception using the provided exception handling service.
                return await _exceptionHandleService.HandleException(ex);   
            }
        }

       
        [HttpGet("getby-countryId")] //  API Endpoint for retrieving data by country ID
        public async Task<IActionResult> GetByCountryIdAsync(int countryId)
        {
            // Checks if the model state is valid
            if (!ModelState.IsValid)
            {
                // Returns a BadRequest response with a list of errors if model state is not valid
                return BadRequest(ModelState.GetErrorList()); 
            }

            // Initializes the response object for returning the result
            var response = new BaseResponseAddResponse<object>();

           

            try
            {
                // Calls the manager to retrieve data by country ID asynchronously
                var data = await _manager.GetByCountryIdAsync(countryId);

                // Checks if the retrieved data is not null
                if (data != null)
                {
                    // Sets StatusCode to 200 indicating success
                    response.StatusCode = 200;
                    response.Data = data;

                    // Returns Ok response with the data
                    return Ok(response);
                }

                // Sets StatusCode to 400 indicating a bad request
                response.StatusCode = 400;
                response.Message = Constants.error;

                // Returns BadRequest response with the error message
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                // Handle the exception using the provided exception handling service.
                return await _exceptionHandleService.HandleException(ex);
            }
        }


        
        [HttpDelete("delete/{id}")] // API  Endpoint for deleting an entity by its ID
        public async Task<IActionResult> DeleteAsync(int id)
        {
            // Checks if the model state is valid
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
                    response.Message = "ID Required";

                    // Returns BadRequest response with the error message
                    return BadRequest(response);
                }

                // Calls the manager to delete the entity asynchronously by its ID
                await _manager.DeleteAsync(id);

                // Sets StatusCode to 200 indicating success
                response.StatusCode = 200;
                response.Message = Constants.deleted;

                // Returns Ok response with the success message
                return Ok(response);

            }
            catch(Exception ex)
            {
                // Handle the exception using the provided exception handling service.
                return await _exceptionHandleService.HandleException(ex); 
            }
        }


    }
}
