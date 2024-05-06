using FHP.infrastructure.DataLayer;
using FHP.infrastructure.Manager.FHP;
using FHP.infrastructure.Service;
using FHP.models.FHP.EmployeeSkillDetail;
using FHP.utilities;
using Microsoft.AspNetCore.Mvc;

namespace FHP.Controllers.FHP
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeSkillDetailController : ControllerBase
    {
        private readonly IEmployeeSkillDetailManager _manager;
        private readonly IExceptionHandleService _exceptionHandleService;
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeSkillDetailController(IEmployeeSkillDetailManager manager,
                                             IExceptionHandleService exceptionHandleService,
                                             IUnitOfWork unitOfWork)
        {
            _manager = manager;
            _exceptionHandleService = exceptionHandleService;
            _unitOfWork = unitOfWork;
        }


        [HttpPost("add")] // Add EmployeeSkill Detail
        public async Task<IActionResult> AddAsync(AddEmployeeSkillDetailModel model)
        {
            if (!ModelState.IsValid)
            {
                //it returns a BadRequest response with a list of errors.y
                return BadRequest(ModelState.GetErrorList());  
            }

            var response = new BaseResponseAdd();

            try
            {

                // Check if all required fields are provided.
                if (model.Id == 0 && model.UserId != 0 && model.SkillId != null)
                {
                    // Add the employee professional detail to the database.
                    await _manager.AddAsync(model);
                    response.StatusCode = 200;
                    response.Message = Constants.added;
                    return Ok(response);

                }

                // If required fields are not provided, return a BadRequest response with an error message.
                response.StatusCode = 400;
                response.Message = Constants.provideValues;
                return BadRequest(response);

            }
            catch(Exception ex)
            {
                //exception handler service
                return await _exceptionHandleService.HandleException(ex);  
            }
        }

        //Edit EmployeeSkillDetail
        [HttpPut("edit")] 
        public async Task<IActionResult> EditAsync(AddEmployeeSkillDetailModel model)
        {
            if (!ModelState.IsValid)
            {
                //it returns a BadRequest response with a list of errors.
                return BadRequest(ModelState.GetErrorList());  
            }

            var response = new BaseResponseAdd();

            //The method then begins a database transaction to ensure data consistency during  updation
            await using var transaction = await _unitOfWork.BeginTransactionAsync(); 

            try
            {

                // Check if the model ID is valid.
                if (model.Id >= 0)
                {
                    // Update the employee professional detail.
                    await _manager.Edit(model);
                    
                    // commit transaction
                    await transaction.CommitAsync(); 
                    response.StatusCode = 200;
                    response.Message = Constants.updated;
                    return Ok(response);
                }

                // If the model ID is invalid, return a BadRequest response with an error message.
                response.StatusCode = 400;
                response.Message = Constants.provideValues;
                return BadRequest(response);

            }
            catch (Exception ex) 
            {
                //In case of any exceptions during the process, it rolls back the transaction.
                await transaction.RollbackAsync(); 

                // exceptionHandler service
                return await _exceptionHandleService.HandleException(ex); 
            }
        }

        // GetAll EmployeeSkill Details with pagination and search filter
        [HttpGet("getall-pagination")]   
        public async Task<IActionResult> GetAllAsync(int page,int pageSize,int userId,string? search, string? skillName)
        {
            if (!ModelState.IsValid)
            {
                //it returns a BadRequest response with a list of errors.
                return BadRequest(ModelState.GetErrorList()); 
            }

            var response = new BaseResponsePagination<object>();

            try
            {
                var data = await _manager.GetAllAsync(page,pageSize,userId,search,skillName);
                if(data.employeeSkillDetail != null)
                {
                    response.StatusCode = 200;
                    response.Data = data.employeeSkillDetail;
                    response.TotalCount = data.totalCount;
                    return Ok(response);
                }

                response.StatusCode = 400;
                response.Message = Constants.error;
                return BadRequest(response);

            }
            catch(Exception ex) 
            {
                //exceptionHandler service
                return await _exceptionHandleService.HandleException(ex); 
            }
        }


        // get by id EmployeeSkillDetail
        [HttpGet("getbyid")]  
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                //it returns a BadRequest response with a list of errors
                return BadRequest(ModelState.GetErrorList());
            }

            var response = new BaseResponseAddResponse<object>();

            try
            {

                // Retrieve data from the manager based on pagination parameters.
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
                // exception handler service
                return await _exceptionHandleService.HandleException(ex); 
            }
        }

        //delete EmployeeskillDetail
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

                if (id <= 0)
                {
                    // If Id is not provided or invalid, return a BadRequest response.
                    response.StatusCode = 400;
                    response.Message = "Id Required";
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
                // exceptionHandler service
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
                if(ids == null)
                {
                    response.StatusCode = 400;
                    response.Message = "Ids Required";
                    return BadRequest(response);
                }

                await _manager.DeleteByIdsAsync(ids);
                response.StatusCode = 200;
                response.Message = Constants.deleted;
                return Ok(response);

            }
            catch(Exception ex)
            {
                return await _exceptionHandleService.HandleException(ex);
            }
        }
    }
}
