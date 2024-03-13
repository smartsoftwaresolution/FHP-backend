
using FHP.infrastructure.DataLayer;
using FHP.infrastructure.Manager.FHP;
using FHP.infrastructure.Service;
using FHP.models.FHP.EmployerContractConfirmation;
using FHP.utilities;
using Microsoft.AspNetCore.Mvc;

namespace FHP.Controllers.FHP
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployerContractConfirmationController : ControllerBase
    {
        private readonly IEmployerContractConfirmationManager _manager;
        private readonly IExceptionHandleService _exceptionHandleService;
        private readonly IUnitOfWork _unitOfWork;

        public EmployerContractConfirmationController(IEmployerContractConfirmationManager manager,
            IExceptionHandleService exceptionHandleService,
            IUnitOfWork unitOfWork)
        {
            _manager=manager;
            _exceptionHandleService=exceptionHandleService; 
            _unitOfWork=unitOfWork;
        }

        [HttpPost("add")]   
        public async Task<IActionResult> AddAsync(AddEmployerContractConfirmationModel model)
        {
            // Checks if the model state is valid
            if (!ModelState.IsValid)
            {
                // Returns a BadRequest response with a list of errors if model state is not valid
                return BadRequest(ModelState.GetErrorList());  
            }

            var response = new BaseResponseAdd();

            //The method then begins a database transaction to ensure data consistency during  addition.
            await using var transaction = await _unitOfWork.BeginTransactionAsync(); 

            try
            {
                // Check if the required fields are provided
                if (model.Id == 0 && model.EmployerId != 0 && model.JobId != 0 && model.EmployeeId != 0)
                {
                    // Add the employerContract
                    await _manager.AddAsync(model); 
                    await transaction.CommitAsync(); 
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

                // Handle the exception using the provided exception handling service
                return await _exceptionHandleService.HandleException(ex); 
            }
        }

        [HttpPut("edit")]  // API endpoint fot Edit EmployerContractConfirmation
        public async Task<IActionResult> EditAsync(AddEmployerContractConfirmationModel model)
        {
            // Check if the model state is valid
            if (!ModelState.IsValid)
            {
                // Return a BadRequest response with a list of errors if model state is not valid
                return BadRequest(ModelState.GetErrorList()); 
            }

            var response = new BaseResponseAdd();

            //The method then begins a database transaction to ensure data consistency during  updation.
            await using var transaction = await _unitOfWork.BeginTransactionAsync(); 

            try
            {
                // Check if the provided model ID is valid
                if (model.Id >= 0)
                {
                    await _manager.Edit(model); 
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

                // Handle the exception using the provided exception handling service
                return await _exceptionHandleService.HandleException(ex);
            }
        }

        [HttpGet("getall-pagination")]  //get all EmployerContractConfirmation
        public async Task<IActionResult> GetAllAsync(int page,int pageSize,string? search)
        {
            // Check if the model state is valid
            if (!ModelState.IsValid)
            {
                // Return a BadRequest response with a list of errors if model state is not valid
                return BadRequest(ModelState.GetErrorList()); 
            }

            var response = new BaseResponsePagination<object>();

            try
            {
                // Retrieve all employer contract  with pagination
                var data = await _manager.GetAllAsync(page,pageSize, search);

                if (data.employerContract != null)
                {
                    response.StatusCode = 200;
                    response.Data = data;
                    response.TotalCount = data.totalCount;
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


        [HttpGet("getbyid")] //get by id EmployerContractConfirmation
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            // Check if the model state is valid
            if (!ModelState.IsValid)
            {
                // Return a BadRequest response with a list of errors if model state is not valid
                return BadRequest(ModelState.GetErrorList()); 
            }

            var response = new BaseResponseAddResponse<object>();

            try
            {
                // Retrieve an employer contract by its ID
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
                // Handle the exception using the provided exception handling service
                return await _exceptionHandleService.HandleException(ex); 
            }
        }


        [HttpDelete("delete/{id}")] //delete EmployerContractConfirmation
        public async Task<IActionResult> DeleteAsync(int id)
        {
            // Check if the model state is valid
            if (!ModelState.IsValid)
            {
                // Return a BadRequest response with a list of errors if model state is not valid
                return BadRequest(ModelState.GetErrorList()); 
            }

            var response = new BaseResponseAdd();

            try
            {
                // Check if the provided ID is valid
                if (id <= 0)
                {
                    response.StatusCode = 400;
                    response.Message = "Id Required.";
                    return BadRequest(response);
                }

                // Delete the employer contract
                await _manager.DeleteAsync(id);
                response.StatusCode = 200;
                response.Message = Constants.deleted;
                return Ok(response);

            }
            catch (Exception ex)
            {
                // Handle the exception using the provided exception handling service
                return await _exceptionHandleService.HandleException(ex);  
            }
        }
    }
}
