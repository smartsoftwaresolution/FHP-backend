﻿
using FHP.entity.UserManagement;
using FHP.infrastructure.DataLayer;
using FHP.infrastructure.Manager.UserManagement;
using FHP.infrastructure.Service;
using FHP.models.UserManagement.User;
using FHP.utilities;
using Microsoft.AspNetCore.Mvc;

namespace FHP.Controllers.UserManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserManager _manager;
        private readonly IExceptionHandleService _exceptionHandleService;
        private readonly IEmailService _emailService;
        private readonly IFileUploadService _fileUploadService;
        private readonly IUnitOfWork _unitOfWork;
        public UserController(IUserManager manager,
                              IExceptionHandleService exceptionHandleService,
                              IEmailService emailService,
                              IFileUploadService  fileUploadService,
                              IUnitOfWork unitOfWork
                              )
        {
            _manager = manager;
            _exceptionHandleService = exceptionHandleService;
            _emailService = emailService;
            _fileUploadService = fileUploadService;
            _unitOfWork = unitOfWork;
        }

     
        [HttpPost("add")] // API Endpoint for adding a new user
        public async Task<IActionResult> AddAsync(AddUserModel model)
        {
            // Checks if the model state is valid
            if (!ModelState.IsValid)
            {
                // Returns a BadRequest response with a list of errors if model state is not valid
                return BadRequest(ModelState.GetErrorList()); 
            }

            // Initializes the response object for returning the result
            var response = new BaseResponseAdd();

            // Begin a database transaction to ensure data consistency during addition.
            await using var transaction = await _unitOfWork.BeginTransactionAsync(); 

            try
            {
                // Checks if required fields are provided and ID is 0 indicating a new user
                if (model.Id == 0 &&
                    !string.IsNullOrEmpty(model.RoleName) &&
                    !string.IsNullOrEmpty(model.Email) &&   
                    !string.IsNullOrEmpty(model.Password))
                {
                    int userid = 0;

                    // Checks if a user with the same email already exists
                    var exist = await _manager.GetUserByEmail(model.Email); 
                    if (exist != null)
                    {
                        // Sets StatusCode to 400 indicating a bad request
                        response.StatusCode = 400;
                        response.Message = "email already exist";

                        // Returns BadRequest response with the error message
                        return BadRequest(response);
                    }

                    // Adds the new user and retrieves the generated user ID
                    userid = await _manager.AddAsync(model);

                    // Sends a verification email to the user
                    await _emailService.SendverificationEmail(model.Email, userid);

                    // Commits the transaction as all operations are successful
                    await transaction.CommitAsync();
                    response.Message = Constants.added;

                    // Returns Ok response with the success message
                    return Ok(response);
                }

                response.StatusCode = 400;
                response.Message = Constants.provideValues;
                return BadRequest(response);

            }
            catch (Exception ex)
            {
                // Rolls back the transaction in case of any exceptions during the process
                await transaction.RollbackAsync();

                // Handle the exception using the provided exception handling service.
                return await _exceptionHandleService.HandleException(ex); 
            }
        }


        
        [HttpPut("edit")] // API Endpoint for editing an existing user
        public async Task<IActionResult> EditAsync(AddUserModel model)
        {
            // Checks if the model state is valid
            if (!ModelState.IsValid)
            {
                // Returns a BadRequest response with a list of errors if model state is not valid
                return BadRequest(ModelState.GetErrorList()); 
            }

            // Initializes the response object for returning the result
            var response = new BaseResponseAdd();

            // Begins a transaction for database operations
            await using var transaction = await _unitOfWork.BeginTransactionAsync(); 


            try
            {
                // Checks if the provided model has a valid ID
                if (model.Id >= 0 )
                {
                    // Calls the manager to edit the existing user asynchronously
                    await _manager.EditAsync(model);

                    // Commits the transaction 
                    await transaction.CommitAsync();

                    // Sets StatusCode to 200 indicating success
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
            catch (Exception ex)
            {
                //In case of any exceptions during the process, it rolls back the transaction.
                await transaction.RollbackAsync();

                // Handle the exception using the provided exception handling service.
                return await _exceptionHandleService.HandleException(ex); 
            }
        }

        
        [HttpGet("getall-pagination")] // get all User with pagination,based on roleName,sorting and searching
        public async Task<IActionResult> GetAllAsync(int page, int pageSize, string? search, string? roleName,bool isAscending)
        {
            // Checks if the model state is valid
            if (!ModelState.IsValid)
            {
                // Returns a BadRequest response with a list of errors if model state is not valid
                return BadRequest(ModelState.GetErrorList());  
            }

            var response = new BaseResponsePagination<object>();


            try
            {
                // Retrieve data from the manager based on pagination parameters.
                var data = await _manager.GetAllAsync(page, pageSize, search, roleName,isAscending);

                if (data.user != null)
                {
                    response.StatusCode = 200;
                    response.Data = data.user;
                    response.TotalCount = data.totalCount;

                    // Returns Ok response with the success message
                    return Ok(response);
                }
               
                response.StatusCode = 400;
                response.Message = Constants.error;

                // Returns BadRequest response with the error message
                return BadRequest(response);

            }
            catch (Exception ex)
            {
                // Handle the exception using the provided exception handling service.
                return await _exceptionHandleService.HandleException(ex); 
            }

        }



        
        [HttpGet("getbyid")] //API Endpoint for retrieving by ID
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            // Checks if the model state is valid
            if (!ModelState.IsValid)
            {
                // Returns a BadRequest response with a list of errors if model state is not valid
                return BadRequest(ModelState.GetErrorList()); 
            }

            var response = new BaseResponseAddResponse<object>();

            try
            {

                var data = await _manager.GetByIdAsync(id);

                // Checks if data is found
                if (data != null)
                {
                    // Sets StatusCode to 200 indicating success
                    response.StatusCode = 200;
                    response.Data = data;

                    // Returns Ok response with the success message
                    return Ok(response);
                }

                response.StatusCode = 400;
                response.Message = Constants.error;

                // Returns BadRequest response with the error message
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                // Handle the exception using the provided exception handling service.
                return await _exceptionHandleService.HandleException(ex); 
            }
        }


        [HttpDelete("delete/{id}")] //API Endpoint for deleting an user by ID
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                // Returns a BadRequest response with a list of errors if model state is not valid
                return BadRequest(ModelState.GetErrorList());
            }

            //Initializes the response object for returning the result
            var response = new BaseResponseAdd();

            try
            {
                // Checks if the provided ID is valid
                if (id <= 0)
                {

                    // Sets StatusCode to 400 indicating a bad request
                    response.StatusCode = 400;
                    response.Message = "ID Required";

                    // Returns BadRequest response with the error message
                    return BadRequest(response);
                }

                // Calls the manager to asynchronously delete the user by ID
                await _manager.DeleteAsync(id);
                response.StatusCode = 200;
                response.Message = Constants.deleted;

                // Returns Ok response with the success message
                return Ok(response);
            }
            catch (Exception ex)
            {

                // Handle any exceptions using the provided exception handling service.
                return await _exceptionHandleService.HandleException(ex); 
            }
        }

        
        [HttpPatch("verify-user")] // API Endpoint for verifying a user
        public async Task<IActionResult> VerifyUserAsync(int userId)
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
                // Checks if a valid user ID is provided
                if (userId > 0)
                {

                    // Calls the manager to verify the user asynchronously
                    await _manager.VerifyUser(userId);

                    // Sets StatusCode to 200 indicating success
                    response.StatusCode = 200;
                    response.Message = "User Verified Successfully!!";

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
                // Handle any exceptions using the provided exception handling service.
                return await _exceptionHandleService.HandleException(ex); 
            }
        }

        
        [HttpPatch("update-Profile-pic")] //  API Endpoint for updating user profile picture
        public async Task<IActionResult> AddProfilePictureAsync(int userId, IFormFile? picUrl)
        {
            // Checks if the model state is valid
            if (!ModelState.IsValid)
            {
                // Returns a BadRequest response with a list of errors if model state is not valid
                return BadRequest(ModelState.GetErrorList()); 
            }

            // Begins a transaction for database operations
            await using var transaction = await _unitOfWork.BeginTransactionAsync();

            // Initializes the response object for returning the result
            var response = new BaseResponseAdd();

            try
            {
                // Checks if a valid user ID is provided
                if (userId < 0)
                {
                    // Sets StatusCode to 400 indicating a bad request
                    response.StatusCode = 400;
                    response.Message = Constants.provideValues;

                    // Returns BadRequest response with the error message
                    return BadRequest(response);
                }

                string pic = string.Empty;

                // Checks if a profile picture is provided
                if (picUrl != null)
                {
                    // Uploads the profile picture and gets the URL
                    pic = await _fileUploadService.UploadIFormFileAsync(picUrl); 
                }

                // Calls the manager to update user's profile picture asynchronously
                await _manager.AddUserPic(userId,pic);

                // Commits the transaction as all operations are successful
                await transaction.CommitAsync();

                // Sets StatusCode to 200 indicating success
                response.StatusCode = 200;
                response.Message = Constants.added;

                // Returns Ok response with the success message
                return Ok(response);
            }
            catch (Exception ex)
            {
                // Handle any exceptions using the provided exception handling service.
                return await _exceptionHandleService.HandleException(ex); 
            }
        }

      
        [HttpPatch("enable-disble-employee-employer")] // API  Endpoint for enabling or disabling
        public async Task<IActionResult> EnableDisableUserAsync(int userId,string roleName)
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
                // Checks if a valid user ID is provided
                if (userId > 0)
                {

                    // Calls the manager to enable or disable the user based on the role name asynchronously
                    string result =  await _manager.EnableDisableUser(userId,roleName);

                    // Sets StatusCode to 200 indicating success
                    response.StatusCode = 200;
                    response.Message = $"User {result} Successfully!!";

                    // Returns Ok response with the success message
                    return Ok(response);
                }
                response.StatusCode = 400;
                response.Message = Constants.provideValues;

                // Returns BadRequest response with the error message
                return BadRequest(response);
            }

            catch (Exception ex)
            {
                // Handle any exceptions using the provided exception handling service.
                return await _exceptionHandleService.HandleException(ex);  
            }
        }

        
        [HttpPatch("verify-employer-by-admin")] //  API Endpoint for verifying an employer by admin
        public async Task<IActionResult> VerifyEmployerByAdminAsync(int userId)
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
                // Checks if a valid user ID is provided
                if (userId > 0)
                {
                    // Calls the manager to verify the employer by admin asynchronously
                    await _manager.VerifyEmployerByAdmin(userId);

                    // Sets StatusCode to 200 indicating success
                    response.StatusCode = 200;
                    response.Message = $"Employer Verified Successfully!!";

                    // Returns Ok response with the success message
                    return Ok(response);
                }
                response.StatusCode = 400;
                response.Message = Constants.provideValues;

                // Returns BadRequest response with the error message
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                // Handle any exceptions using the provided exception handling service.
                return await _exceptionHandleService.HandleException(ex);    
            }
        }

    }
}