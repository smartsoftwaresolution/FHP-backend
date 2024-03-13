
using FHP.infrastructure.DataLayer;
using FHP.infrastructure.Manager.FHP;
using FHP.infrastructure.Service;
using FHP.models.FHP.EmployeeDetail;
using FHP.utilities;
using Microsoft.AspNetCore.Mvc;

namespace FHP.Controllers.FHP
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeDetailController : ControllerBase
    {
        private readonly IEmployeeDetailManager _manager;
        private readonly IExceptionHandleService _exceptionHandleService;
        private readonly IFileUploadService _fileUploadService;
        private readonly IUnitOfWork _unitOfWork;
        public EmployeeDetailController(IEmployeeDetailManager manager,
                                        IExceptionHandleService exceptionHandleService,
                                        IFileUploadService fileUploadService,
                                        IUnitOfWork unitOfWork)
        {
            _manager = manager;
            _exceptionHandleService = exceptionHandleService;
            _fileUploadService = fileUploadService;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("add")]  // Add Employee Detail 
        public async Task<IActionResult> AddAsync([FromForm]AddEmployeeDetailModel model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList()); //it returns a BadRequest response with a list of errors.
            }

            var response = new BaseResponseAdd();
            await using var transaction = await _unitOfWork.BeginTransactionAsync(); //The method then begins a database transaction to ensure data consistency during  addition.

            try
            {
                if(model.Id == 0 &&  model.UserId != 0 && model.CountryId != 0 && model.StateId != 0  && model.CityId != 0
                   && !string.IsNullOrEmpty(model.MaritalStatus)
                   && !string.IsNullOrEmpty(model.Gender)
                   && !string.IsNullOrEmpty(model.PermanentAddress)
                   && !string.IsNullOrEmpty(model.Mobile))
                   
                {
                    string profileImg = string.Empty;

                    string profileResume = string.Empty;

                    if (model.ProfileImgURL != null)
                    {
                      profileImg =  await _fileUploadService.UploadIFormFileAsync(model.ProfileImgURL); // Profile Image Upload service
                    }

                    if (model.ResumeURL != null)
                    {
                        profileResume = await _fileUploadService.UploadIFormFileAsync(model.ResumeURL); //ResumeUrl Upload service
                    }

                    await _manager.AddAsync(model,profileResume);  // Added
                    await transaction.CommitAsync(); //commit the transaction.

                    response.StatusCode = 200;
                    response.Message = Constants.added;
                    return Ok(response);
                }

                response.StatusCode = 400;
                response.Message= Constants.provideValues;
                return BadRequest(response);

            }
            catch(Exception ex) 
            { 
                await transaction.RollbackAsync(); //In case of any exceptions during the process, it rolls back the transaction.
                return await _exceptionHandleService.HandleException(ex); //exceptionHandle service.
            }
        }


        [HttpPut("edit")]  //  Edit Employee Detail
        public async Task<IActionResult> EditAsync([FromForm] AddEmployeeDetailModel model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList()); //it returns a BadRequest response with a list of errors.
            }

            await using var transaction = await _unitOfWork.BeginTransactionAsync(); //The method then begins a database transaction to ensure data consistency during  updation.
            var response = new BaseResponseAdd();

            try
            {
                if(model.Id >= 0)
                {
                    string resumeUrl = string.Empty;

                    if (model.ResumeURL != null)
                    {
                        resumeUrl = await _fileUploadService.UploadIFormFileAsync(model.ResumeURL); //Resume Upload Service
                    }

                    await _manager.Edit(model,resumeUrl); //Updated
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
                await transaction.RollbackAsync(); //In case of any exceptions during the process, it rolls back the transaction.
                return await _exceptionHandleService.HandleException(ex); //exceptionHandler service
            }
        }

        [HttpGet("getall-pagination")]  // Get all List of Employee Detail 
        public async Task<IActionResult> GetAllAsync(int page,int pagesize,int userId,string? search)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());   //it returns a BadRequest response with a list of errors.
            } 

            var response = new BaseResponsePagination<object>();

            try
            {
                var data = await _manager.GetAllAsync(page,pagesize,userId,search);
                if(data.employee != null)
                {
                    response.StatusCode = 200;
                    response.Data = data.employee;
                    response.TotalCount = data.totalCount;
                    return Ok(response);
                }

                response.StatusCode = 400;
                response.Message = Constants.error;
                return BadRequest(response);

            }
            catch(Exception ex)
            {
                return await _exceptionHandleService.HandleException(ex);  //exceptionHandler service
            }
        }

        [HttpGet("getbyid")] // Get By Id employee detail
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList()); //it returns a BadRequest response with a list of errors.
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
                return await _exceptionHandleService.HandleException(ex); //exception handlerservice
            }
        }


        [HttpDelete("delete/{id}")] // Delete by Id
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());    //it returns a BadRequest response with a list of errors.
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
                return await _exceptionHandleService.HandleException(ex); // exception handlerservice
            }
        }

        [HttpPatch("set-availability/{id}")] // Employee Can Set Availability
        public async Task<IActionResult> SetAvailabilityAsync(int id)
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
                    response.Message = "Id Required";
                    return BadRequest(response);
                }

                string result = await _manager.SetAvailabilityAsync(id);
                response.StatusCode = 200;
                response.Message = $"Employee {result} Now!!";
                return Ok(response);
            }
            catch (Exception ex)
            {
                return await _exceptionHandleService.HandleException(ex); // exception handlerservice
            }
        }

        [HttpGet("getall-by-id")]  // get all the employee detail
        public async Task<IActionResult> GetAllByIdAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList()); //it returns a BadRequest response with a list of errors.
            }

            var response = new BaseResponseAddResponse<object>();

            try
            {
                var data = await _manager.GetAllByIdAsync(id);
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
                return await _exceptionHandleService.HandleException(ex);  // exception handlerservice
            }
        }

    }
}
