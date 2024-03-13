using FHP.infrastructure.DataLayer;
using FHP.infrastructure.Manager.UserManagement;
using FHP.infrastructure.Service;
using FHP.models.UserManagement.Country;
using FHP.utilities;
using Microsoft.AspNetCore.Mvc;

namespace FHP.Controllers.UserManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {

        private readonly IExceptionHandleService _exceptionHandleService;
        private readonly ICountryManager _manager;
        private readonly IUnitOfWork _unitOfWork;

        public CountryController(ICountryManager manager,
                                 IExceptionHandleService exceptionHandleService,
                                 IUnitOfWork unitOfWork)
        {
            _exceptionHandleService = exceptionHandleService;
            _manager = manager;
            _unitOfWork = unitOfWork;
        }



        [HttpPost("add")]
        public async Task<IActionResult> AddAsync(AddCountryModel model)
        {
            // Checks if the model state is valid
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
                // Validates the required fields for adding country
                if (model.Id == 0 && !string.IsNullOrEmpty(model.CountryName))
                {
                    // Calls the manager to add country asynchronously
                    await _manager.AddAsync(model);
                    await transaction.CommitAsync(); // commit transaction
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
                //In case of any exceptions during the process, it rolls back the transaction
                await transaction.RollbackAsync();

                // Handle the exception using the provided exception handling service.
                return await _exceptionHandleService.HandleException(ex);
            }
        }



        [HttpPut("edit")]
        public async Task<IActionResult> EditAsync(AddCountryModel model)
        {
            // Checks if the model state is valid
            if (!ModelState.IsValid)
            {
                //it returns a BadRequest response with a list of errors.
                return BadRequest(ModelState.GetErrorList());
            }

            var response = new BaseResponseAdd();

            //The method then begins a database transaction to ensure data consistency during  updation.
            await using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                // Checks if the model ID is greater than or equal to 0
                if (model.Id >= 0 && model != null)
                { 
                    await _manager.Edit(model);

                    // commmit transaction
                    await transaction.CommitAsync(); 

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
                //In case of any exceptions during the process, it rolls back the transaction.
                await transaction.RollbackAsync();

                // Handle the exception using the provided exception handling service.
                return await _exceptionHandleService.HandleException(ex);
            }
        }

            
            [HttpGet("getall-pagination")]
            public async Task<IActionResult> GetAllAsync(int page, int pageSize, string? search)
            {
                  // Checks if the model state is valid
                if (!ModelState.IsValid)
                {
                  //it returns a BadRequest response with a list of errors.
                  return BadRequest(ModelState.GetErrorList()); 
                }

                var response = new BaseResponsePagination<object>();

                try
                {

                // Calls the manager to retrieve country asynchronously with pagination and search
                var data = await _manager.GetAllAsync(page, pageSize, search);

                // Checks if the retrieved data is not null
                if (data.country != null)
                    {
                        response.StatusCode = 200;
                        response.Data = data.country;
                        response.TotalCount = data.totalCount;

                    // Returns Ok response with the data
                    return Ok(response);
                    }

                    response.StatusCode = 400;
                    response.Message = Constants.error;
                    return BadRequest(response);
                }
                catch (Exception ex)
                {

                // Handle the exception using the provided exception handling service
                  return await _exceptionHandleService.HandleException(ex); 
                }
            }


           
            [HttpGet("getbyid")]
            public async Task<IActionResult> GetByIdAsync(int id)
            {
                // Checks if the model state is valid
                if (!ModelState.IsValid)
                {
                 //it returns a BadRequest response with a list of errors.
                 return BadRequest(ModelState.GetErrorList()); 
                }

                var respone = new BaseResponseAddResponse<object>();

                try
                {
                // Calls the manager to retrieve an entity by its ID asynchronously
                var data = await _manager.GetByIdAsync(id);


                // Checks if the retrieved data is not null
                if (data != null)
                    {
                        respone.StatusCode = 200;
                        respone.Data = data;
                        return Ok(respone);
                    }

                    respone.StatusCode = 400;
                    respone.Message = Constants.error;
                    return BadRequest(respone);
                }
                catch (Exception ex)
                {
                // Handle the exception using the provided exception handling service.
                return await _exceptionHandleService.HandleException(ex); 
                }

            }


           
            [HttpDelete("delete/{id}")]
            public async Task<IActionResult> DeleteAsync(int id)
            {
            // Checks if the model state is valid
                if (!ModelState.IsValid)
                {
                   // Returns a BadRequest response with a list of errors if model state is not valid
                    return BadRequest(ModelState.GetErrorList());
                }

                var response = new BaseResponseAdd();

                try
                {
                // Checks if the provided ID is valid
                    if (id <= 0)
                    {
                        response.StatusCode = 400;
                        response.Message = "Id required.";
                        return BadRequest(response);
                    }

                    // Calls the manager to delete the entity asynchronously by its ID
                    await _manager.DeleteAsync(id);
                    response.StatusCode = 200;
                    response.Message = Constants.deleted;

                      // Returns Ok response with the success message
                      return Ok(response);
                }
                catch (Exception ex)
                {
                   // Handle the exception using the provided exception handling service.
                    return await _exceptionHandleService.HandleException(ex); 
                }
            }
        }
    }


