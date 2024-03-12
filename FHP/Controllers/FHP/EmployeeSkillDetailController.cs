using FHP.infrastructure.DataLayer;
using FHP.infrastructure.Manager.FHP;
using FHP.infrastructure.Service;
using FHP.models.FHP;
using FHP.services;
using FHP.utilities;
using Microsoft.AspNetCore.Http;          
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
                return BadRequest(ModelState.GetErrorList());
            }

            var response = new BaseResponseAdd();
          //  await using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                if(model.Id == 0 && model.UserId != 0 && model.SkillId != null)
                {

                    await _manager.AddAsync(model);
                 //   await transaction.CommitAsync();

                  

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
             //   await transaction.RollbackAsync();
                return await _exceptionHandleService.HandleException(ex);
            }
        }


        [HttpPut("edit")] //Edit EmployeeSkillDetail
        public async Task<IActionResult> EditAsync(AddEmployeeSkillDetailModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }

            var response = new BaseResponseAdd();
            await using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                if(model.Id >= 0)
                {
                    await _manager.Edit(model); // updated
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
                await transaction.RollbackAsync();
                return await _exceptionHandleService.HandleException(ex);
            }
        }

        [HttpGet("getall-pagination")]   // GetAll EmployeeSkill Details
        public async Task<IActionResult> GetAllAsync(int page,int pageSize,int userId,string? search)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }

            var response = new BaseResponsePagination<object>();

            try
            {
                var data = await _manager.GetAllAsync(page,pageSize,userId,search);
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
                return await _exceptionHandleService.HandleException(ex);
            }
        }

        [HttpGet("getbyid")]   // get by id EmployeeSkillDetail
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }

            var response = new BaseResponseAddResponse<object>();

            try
            {
                var data = await _manager.GetByIdAsync(id);
                if(data != null)
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
                return await _exceptionHandleService.HandleException(ex);
            }
        }

        [HttpDelete("delete/{id}")]  //delete EmployeeskillDetail
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }

            var response = new BaseResponseAdd();

            try
            {
                if(id <= 0)
                {
                    response.StatusCode = 400;
                    response.Message = "Id Required";
                    return BadRequest(response);
                }

                await _manager.DeleteAsync(id);
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
