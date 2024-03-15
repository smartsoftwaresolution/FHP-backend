using FHP.infrastructure.DataLayer;
using FHP.infrastructure.Manager.UserManagement;
using FHP.infrastructure.Service;
using FHP.models.UserManagement.Screen;
using FHP.utilities;
using Microsoft.AspNetCore.Mvc;

namespace FHP.Controllers.UserManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScreenController : ControllerBase
    {
        private readonly IScreenManager _manager;
        private readonly IExceptionHandleService _exceptionHandleService;
        private readonly IUnitOfWork _unitOfWork;
        public ScreenController(IScreenManager manager,
                                IExceptionHandleService exceptionHandleService,
                                IUnitOfWork unitOfWork)
        {
            _manager = manager;
            _exceptionHandleService= exceptionHandleService;
            _unitOfWork = unitOfWork;
        }


        //  API Endpoint for add  screen
        [HttpPost("add")] 
        public async Task<IActionResult> AddAsync(AddScreenModel model)
        {
            if (!ModelState.IsValid)
            {
                // Returns a BadRequest response with a list of errors if model state is not valid
                return BadRequest(ModelState.GetErrorList()); 
            }

            // Initializes the response object for returning the result
            var response = new BaseResponseAdd();

            //The method then begins a database transaction to ensure data consistency during  addition.
            await using var transaction = await _unitOfWork.BeginTransactionAsync(); 

            try
            {
                // Checks if the provided model contains necessary data for addition
                if (model.Id==0  && 
                    !string.IsNullOrEmpty(model.ScreenName) && 
                    !string.IsNullOrEmpty(model.ScreenCode))
                {
                    await _manager.AddAsync(model);

                    // commit transaction
                    await transaction.CommitAsync(); 
                    response.StatusCode = 200;
                    response.Message = Constants.added;

                    // Returns Ok response with the success message
                    return Ok(response);
                }

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


        //  API Endpoint for edit screen
        [HttpPut("edit")]
        public async Task<IActionResult> EditAsync(AddScreenModel model)
        {
            if (!ModelState.IsValid)
            {
                // Returns a BadRequest response with a list of errors if model state is not valid
                return BadRequest(ModelState.GetErrorList()); 
            }

            //The method then begins a database transaction to ensure data consistency during  updation.
            await using var transaction = await _unitOfWork.BeginTransactionAsync();


            // Initializes the response object for returning the result
            var response = new BaseResponseAdd();

            try
            {
               
                if (model.Id >= 0 && model != null)
                {
                    await _manager.EditAsync(model);
                    await transaction.CommitAsync(); 
                    response.StatusCode = 200;
                    response.Message = Constants.updated;

                    // Returns Ok response with the success message
                    return Ok(response);
                }

                response.StatusCode = 400;
                response.Message = Constants.provideValues;

                // Returns BadRequest response with the error message
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

        //  API Endpoint for retrieving all screens with pagination support
        [HttpGet("getall-pagination")]
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

                // Calls the manager to retrieve all screens asynchronously with pagination and search
                var data =  await _manager.GetAllAsync(page,pageSize,search);

                // Checks if the returned data is not null
                if (data.screen != null)
                {
                    response.StatusCode = 200;
                    response.Data = data.screen;
                    response.TotalCount = data.totalCount;

                    // Returns Ok response with the retrieved screens data
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


        // API Endpoint for retrieving an entity by its ID
        [HttpGet("getbyid")]
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
                var data=await _manager.GetByIdAsync(id);

                // Checks if the returned data is not null
                if (data != null)
                {
                    response.StatusCode = 200;
                    response.Data = data;

                   // Returns Ok response with the retrieved screens data
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

        // API  Endpoint for deleting an entity by its ID
        [HttpDelete("delete/{id}")] 
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
                if (id <= 0)
                {
                    // Sets StatusCode to 400 indicating a bad request
                    response.StatusCode = 200;
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
