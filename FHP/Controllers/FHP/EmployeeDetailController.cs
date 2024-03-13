
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

                    // Upload profile image if provided.
                    if (model.ProfileImgURL != null)
                    {
                      profileImg =  await _fileUploadService.UploadIFormFileAsync(model.ProfileImgURL); // Profile Image Upload service
                    }
                    // Upload resume if provided.
                    if (model.ResumeURL != null)
                    {
                        profileResume = await _fileUploadService.UploadIFormFileAsync(model.ResumeURL); //ResumeUrl Upload service
                    }
                  
                    // Add Employee Detail asynchronously.
                    await _manager.AddAsync(model,profileResume);
                    
                    //commit the transaction.
                    await transaction.CommitAsync(); 

                    response.StatusCode = 200;
                    response.Message = Constants.added;
                    return Ok(response);
                }
                // If required values are not provided, return a BadRequest response.
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
                // Check if the provided model has a valid Id.
                if (model.Id >= 0)
                {
                    string resumeUrl = string.Empty;
                    // Upload resume if provided.
                    if (model.ResumeURL != null)
                    {
                        resumeUrl = await _fileUploadService.UploadIFormFileAsync(model.ResumeURL); //Resume Upload Service
                    }
                   
                    // Edit Employee Detail asynchronously.
                    await _manager.Edit(model,resumeUrl);
                   
                    //commit transaction
                    await transaction.CommitAsync(); //commit transaction
                    response.StatusCode = 200;
                    response.Message = Constants.updated;
                    return Ok(response);
                }

                // Return OK response with the success message.
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
                // Retrieve data from the manager based on pagination parameters.
                var data = await _manager.GetAllAsync(page,pagesize,userId,search);

                // Check if data is retrieved successfully.
                if (data.employee != null)
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

                // Retrieve Employee data by its Id from the manager.
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
                    // If Id is not provided or invalid, return a BadRequest response.
                    response.StatusCode = 400;
                    response.Message = "Id Required";
                    return BadRequest(response);    
                }

                // Delete employeeDetail asynchronously using the manager.
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
                // Check if the provided employee Id is valid.
                if (id <= 0)
                {
                    // If Id is not provided, return a BadRequest response.
                    response.StatusCode = 400;
                    response.Message = "Id Required";
                    return BadRequest(response);
                }
                // Set the availability for the employee with the provided Id.
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
                // Retrieve all employee details by the provided Id.
                var data = await _manager.GetAllByIdAsync(id);
                if (data != null)
                {
                    // If data is found, set response status code to 200 and return the data.
                    response.StatusCode = 200;
                    response.Data = data;
                    return Ok(response);
                }
                // If no data found, return a BadRequest response with an error message.
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
