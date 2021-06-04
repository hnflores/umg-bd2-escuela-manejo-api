using API_ESC_MANEJO.CORE.CORE.Enumerations;
using API_ESC_MANEJO.CORE.Entities;
using API_ESC_MANEJO.CORE.Entities.Configuration;
using API_ESC_MANEJO.CORE.Entities.Response;
using API_ESC_MANEJO.CORE.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_ESC_MANEJO.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ConfigurationMessages _messagesDefault;
        private readonly ILogService _logService;
        private readonly ICustomerService _customerService;
        private readonly ISecurityService _securityService;
        public CustomerController(ILogService logService, IOptions<ConfigurationMessages> messagesDefault
          , ISecurityService securityService, ICustomerService customerService)
        {
            _logService = logService;
            _securityService = securityService;
            _messagesDefault = messagesDefault.Value;
            _customerService = customerService;
        }

        [Route(nameof(GetCustomers)), HttpPost]
        public async Task<ResponseAPI<List<Customer>>> GetCustomers()
        {
            ResponseAPI<List<Customer>> responseAPI = new();
            try
            {
                _logService.SaveLogApp($"[{nameof(GetCustomers)} Request]", LogType.Information);
                #region AUTH
                if (!_securityService.ValidAuth(HttpContext.Request.Headers["Key-Auth"]))
                {
                    _logService.SaveLogApp($"[{nameof(GetCustomers)} InvalidAuth]", LogType.Information);
                    responseAPI.Code = ResponseCode.Error;
                    responseAPI.Description = _messagesDefault.AuthInvalidMessage;
                    return responseAPI;
                }
                #endregion                
                responseAPI = await _customerService.GetCustomers();
                _logService.SaveLogApp($"[{nameof(GetCustomers)} Response] {JsonConvert.SerializeObject(responseAPI)}", LogType.Information);
            }
            catch (Exception ex)
            {
                responseAPI.Code = ResponseCode.FatalError;
                responseAPI.Description = _messagesDefault.FatalErrorMessage;
                _logService.SaveLogApp($"[{nameof(CustomerController)}  {nameof(GetCustomers)} Exception] {ex.Message}|{ex.StackTrace}", LogType.Error);
            }
            return responseAPI;
        }

        [Route(nameof(GetCustomerById)), HttpPost]
        public async Task<ResponseAPI<Customer>> GetCustomerById(Customer request)
        {
            ResponseAPI<Customer> responseAPI = new();
            try
            {
                _logService.SaveLogApp($"[{nameof(GetCustomerById)} Request] {JsonConvert.SerializeObject(request)}", LogType.Information);
                #region AUTH
                if (!_securityService.ValidAuth(HttpContext.Request.Headers["Key-Auth"]))
                {
                    _logService.SaveLogApp($"[{nameof(GetCustomerById)} InvalidAuth]", LogType.Information);
                    responseAPI.Code = ResponseCode.Error;
                    responseAPI.Description = _messagesDefault.AuthInvalidMessage;
                    return responseAPI;
                }
                #endregion                
                responseAPI = await _customerService.GetCustomerById(request.CustomerId);
                _logService.SaveLogApp($"[{nameof(GetCustomerById)} Response] {JsonConvert.SerializeObject(responseAPI)}", LogType.Information);
            }
            catch (Exception ex)
            {
                responseAPI.Code = ResponseCode.FatalError;
                responseAPI.Description = _messagesDefault.FatalErrorMessage;
                _logService.SaveLogApp($"[{nameof(CustomerController)}  {nameof(GetCustomerById)} Exception] {ex.Message}|{ex.StackTrace}", LogType.Error);
            }
            return responseAPI;
        }
    }
}
