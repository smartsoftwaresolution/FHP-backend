
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

        [HttpPost("add")]   // Add EmployerContractConfirmation
        public async Task<IActionResult> AddAsync(AddEmployerContractConfirmationModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());  //it returns a BadRequest response with a list of errors.
            }

            var response = new BaseResponseAdd();
            await using var transaction = await _unitOfWork.BeginTransactionAsync();  //The method then begins a database transaction to ensure data consistency during  addition.

            try
            {
                if (model.Id == 0 && model.EmployerId != 0 && model.JobId != 0 && model.EmployeeId != 0)
                {
                    await _manager.AddAsync(model); // Added 
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
                await transaction.RollbackAsync(); //In case of any exceptions during the process, it rolls back the transaction.
                return await _exceptionHandleService.HandleException(ex); //exception hadler service
            }
        }

        [HttpPut("edit")]  //Edit EmployerContractConfirmation
        public async Task<IActionResult> EditAsync(AddEmployerContractConfirmationModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList()); //it returns a BadRequest response with a list of errors.
            }

            var response = new BaseResponseAdd();
            await using var transaction = await _unitOfWork.BeginTransactionAsync(); //The method then begins a database transaction to ensure data consistency during  updation.

            try
            {
                if (model.Id >= 0)
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
            catch (Exception ex)
            {
                await transaction.RollbackAsync(); //In case of any exceptions during the process, it rolls back the transaction.
                return await _exceptionHandleService.HandleException(ex); //exception handler service
            }
        }

        [HttpGet("getall-pagination")]  //get all EmployerContractConfirmation
        public async Task<IActionResult> GetAllAsync(int page,int pageSize,string? search)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList()); //it returns a BadRequest response with a list of errors.
            }

            var response = new BaseResponsePagination<object>();

            try
            {
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
                return await _exceptionHandleService.HandleException(ex); // exceptionHandler service
            }
        }


        [HttpGet("getbyid")] //get by id EmployerContractConfirmation
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
                return await _exceptionHandleService.HandleException(ex); // exceptionHandler service
            }
        }


        [HttpDelete("delete/{id}")] //delete EmployerContractConfirmation
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
                    response.Message = "Id Required.";
                    return BadRequest(response);
                }

                await _manager.DeleteAsync(id);
                response.StatusCode = 200;
                response.Message = Constants.deleted;
                return Ok(response);

            }
            catch (Exception ex)
            { 
                return await _exceptionHandleService.HandleException(ex);  //exceptionHandler service
            }
        }
    }
}
