﻿using FHP.infrastructure.Manager.UserManagement;
using FHP.infrastructure.Service;
using FHP.models.UserManagement;
using FHP.utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FHP.Controllers.UserManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailSettingController : ControllerBase
    {
        private readonly IEmailSettingManager _manager;
        private readonly IExceptionHandleService _exceptionHandleService;
        public EmailSettingController(IEmailSettingManager manager, IExceptionHandleService exceptionHandleService)
        {
            _manager = manager;
            _exceptionHandleService = exceptionHandleService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddAsync(AddEmailSettingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }

            var response = new BaseResponseAdd();

            try
            {
                var header = Request.Headers["CompanyId"];
                int companyId = Convert.ToInt32(header);

                if(model.Id==0 && companyId !=0 &&
                   !string.IsNullOrEmpty(model.Email) &&
                   !string.IsNullOrEmpty(model.Password) &&
                   !string.IsNullOrEmpty(model.AppPassword) &&
                   !string.IsNullOrEmpty(model.IMapHost) &&
                   !string.IsNullOrEmpty(model.IMapPort) &&
                   !string.IsNullOrEmpty(model.SmtpHost)&&
                   !string.IsNullOrEmpty(model.SmtpPort))                
                {
                    await _manager.AddAsync(model, companyId);
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
                return await _exceptionHandleService.HandleException(ex);
            }
        }

        [HttpPut("edit")]
        public async Task<IActionResult> EditAsync(AddEmailSettingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }
            
            var response = new BaseResponseAdd();

            try
            {
                var header = Request.Headers["CompanyId"];
                int companyId = Convert.ToInt32(header);

                if(model.Id>=0 && model != null)
                {
                    await _manager.EditAsync(model, companyId);
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
                return await _exceptionHandleService.HandleException(ex);
            }
        }


        [HttpGet("getall")]
        public async Task<IActionResult> GetAllAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }

            var response = new BaseResponseAddResponse<object>();

            try
            {
                var header = Request.Headers["CompanyId"];
                int companyId = Convert.ToInt32(header);

                var data=await _manager.GetAllAsync(companyId);
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
            catch(Exception ex)
            {
                return await _exceptionHandleService.HandleException(ex);   
            }
        }

        [HttpGet("getbyid")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }

            var response = new BaseResponseAddResponse<object>();

            try
            {
                var header = Request.Headers["CompanyId"];
                int companyId=Convert.ToInt32(header);
                var data = await _manager.GetByIdAsync(id, companyId);

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
            catch(Exception ex)
            {

                return await _exceptionHandleService.HandleException(ex);
            }
        }


        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }

            var response = new BaseResponseAdd();
            try
            {
                var header = Request.Headers["CompanyId"];
                int companyId=Convert.ToInt32(header);

                if (id <= 0)
                {
                    response.StatusCode = 400;
                    response.Message = "ID Required";
                    return BadRequest(response);
                }
                await _manager.DeleteAsync(id, companyId);
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