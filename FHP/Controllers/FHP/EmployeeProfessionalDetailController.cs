using FHP.infrastructure.DataLayer;
using FHP.infrastructure.Manager.FHP;
using FHP.infrastructure.Service;
using FHP.models.FHP.EmployeeProfessionalDetail;
using FHP.utilities;
using Microsoft.AspNetCore.Mvc;

namespace FHP.Controllers.FHP
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeProfessionalDetailController : ControllerBase
    {
        private readonly IEmployeeProfessionalDetailManager _manager;
        private readonly IExceptionHandleService _exceptionHandleService;
        private readonly IUnitOfWork _unitOfWork;
        public EmployeeProfessionalDetailController(IEmployeeProfessionalDetailManager manager,
                                                    IExceptionHandleService exceptionHandleService,
                                                    IUnitOfWork unitOfWork)
        {
            _manager=manager;
            _exceptionHandleService=exceptionHandleService;
            _unitOfWork=unitOfWork;
        }


        // Add EmployeeProfessionalDetail
        [HttpPost("add")] 
        public async Task<IActionResult> AddAsync(AddEmployeeProfessionalDetailModel model)
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
                // Check if all required fields are provided.
                if (model.Id == 0 && model.UserId != 0 && model.YearsOfExperience != 0 && 
                    !string.IsNullOrEmpty(model.JobDescription) &&
                    !string.IsNullOrEmpty(model.StartDate) &&
                    !string.IsNullOrEmpty(model.EndDate) && 
                    !string.IsNullOrEmpty(model.CompanyName) && 
                    !string.IsNullOrEmpty(model.CompanyLocation) &&
                    !string.IsNullOrEmpty(model.Designation) &&
                    !string.IsNullOrEmpty(model.EmploymentStatus))
                {
                    // Add the employee professional detail to the database.
                    await _manager.AddAsync(model);

                    //commit transaction
                    await transaction.CommitAsync(); 
                    response.StatusCode = 200;

                    response.Message = Constants.added;
                    return Ok(response);

                }

                // If required fields are not provided, return a BadRequest response with an error message.
                response.StatusCode = 400;
                response.Message = Constants.provideValues;
                return BadRequest(response);    
            }
            catch (Exception ex)
            {
                //In case of any exceptions during the process, it rolls back the transaction.
                await transaction.RollbackAsync();

                //exception hadler service
                return await _exceptionHandleService.HandleException(ex); 
            }
        }

        // Edit EmployeeProfessionalDetail
        [HttpPut("edit")]  
        public async Task<IActionResult> EditAsync(AddEmployeeProfessionalDetailModel model)
        {
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
            catch(Exception ex)
            {
                //In case of any exceptions during the process, it rolls back the transaction.
                await transaction.RollbackAsync(); 

                return await _exceptionHandleService.HandleException(ex); 
            }
        }

        // Get All EmployeeProfessionalDetail 
        [HttpGet("getall-pagination")] 
        public async Task<IActionResult> GetAllAsync(int page,int pageSize,int userId,string? search)
        {
            if (!ModelState.IsValid)
            {
                //it returns a BadRequest response with a list of errors.
                return BadRequest(ModelState.GetErrorList()); 
            }

            var response = new BaseResponsePagination<object>();

            try
            {
                var data = await _manager.GetAllAsync(page, pageSize,userId, search);
                if(data.employeeProfessionalDetail != null)
                {
                    response.StatusCode = 200;
                    response.Data = data.employeeProfessionalDetail;
                    response.TotalCount = data.totalCount;
                    return Ok(response);
                }


                response.StatusCode = 400;
                response.Message = Constants.error;
                return BadRequest(response);

            }
            catch( Exception ex)
            {
                // exception handler service
                return await _exceptionHandleService.HandleException(ex); 
            }
        }

        // get by id EmployeeProfessionalDetail
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
                 // Retrieve EmployeeProfessionalDetail data by its Id from the manager.
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
            catch (Exception ex)
            {
                // exception handler service
                return await _exceptionHandleService.HandleException(ex); 
            }
        }

        //delete EmployeeProfessionalDetail by id
        [HttpDelete("delete/{id}")] 
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if(!ModelState.IsValid) 
            {
                //it returns a BadRequest response with a list of errors.
                return BadRequest(ModelState.GetErrorList()); 
            }

            var response = new BaseResponseAdd();

            try
            {
                //Check Proper Id
                if(id <= 0)
                {
                    response.StatusCode = 400;
                    response.Message = "Id Required";
                    return BadRequest(response);
                }

                // Delete EmployeeProfessionalDetail asynchronously using the manager.
                await _manager.DeleteAsync(id);
                response.StatusCode = 200;
                response.Message = Constants.deleted;
                return Ok(response);

            }
            catch(Exception ex)
            {
                // exception handler service
                return await _exceptionHandleService.HandleException(ex); 
            }
        }
    }
}
