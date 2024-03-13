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
            _exceptionHandleService= exceptionHandleService;
            _manager = manager;
            _unitOfWork = unitOfWork;
        }


        // Add Country
        [HttpPost("add")]
        public async Task<IActionResult> AddAsync(AddCountryModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList()); //it returns a BadRequest response with a list of errors.
            }

            var response = new BaseResponseAdd();
            await using var transaction = await _unitOfWork.BeginTransactionAsync(); //The method then begins a database transaction to ensure data consistency during  addition.

            try
            {
                if(model.Id ==0 && !string.IsNullOrEmpty(model.CountryName))
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
                await transaction.RollbackAsync(); //In case of any exceptions during the process, it rolls back the transaction
                return await _exceptionHandleService.HandleException(ex); //exceptionHandler service
            } 
        }


        // edit Country
        [HttpPut("edit")]
        public async Task<IActionResult> EditAsync(AddCountryModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList()); //it returns a BadRequest response with a list of errors.
            } 

            var response = new BaseResponseAdd();
            await using var transaction = await _unitOfWork.BeginTransactionAsync(); //The method then begins a database transaction to ensure data consistency during  updation.
            try
            {
                if(model.Id>=0 && model != null)
                {
                    await _manager.Edit(model); //updated
                    await transaction.CommitAsync(); // commmit transaction
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
               await transaction.RollbackAsync(); //In case of any exceptions during the process, it rolls back the transaction.
                return  await _exceptionHandleService.HandleException(ex);  // exceptionHandler service
            }
        }

        //get all Country
        [HttpGet("getall-pagination")]
        public async Task<IActionResult> GetAllAsync(int page,int pageSize,string? search)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList()); //it returns a BadRequest response with a list of errors.
            }

            var response =new BaseResponsePagination<object>();

            try
            {
                var data = await _manager.GetAllAsync(page,pageSize,search);
                if (data.country !=null)
                {
                    response.StatusCode = 200;
                    response.Data = data.country;
                    response.TotalCount = data.totalCount;
                    return Ok(response);
                }

                response.StatusCode = 400;
                response.Message = Constants.error;
                return BadRequest(response);
            }
            catch(Exception ex)
            {
                return await _exceptionHandleService.HandleException(ex); // exceptionHandler service
            }
        }


        //get by id Country
        [HttpGet("getbyid")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList()); //it returns a BadRequest response with a list of errors.
            }

            var respone =new BaseResponseAddResponse<object>();

            try
            {
                var data = await _manager.GetByIdAsync(id);
                if(data !=null)
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
                return await _exceptionHandleService.HandleException(ex); // exceptionHandler service
            }

        }


        //delete Country
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
                    response.Message = "Id required.";
                    return BadRequest(response);
                }
                await _manager.DeleteAsync(id);
                response.StatusCode = 200;
                response.Message = Constants.deleted;
                return Ok(response);
            }
            catch(Exception ex)
            {
                return await _exceptionHandleService.HandleException(ex); // exception Handler service
            }
        }
    }
}
