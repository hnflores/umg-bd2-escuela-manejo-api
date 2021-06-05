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
    public class ContractController : ControllerBase
    {

        private readonly ConfigurationMessages _messagesDefault;
        private readonly ILogService _logService;
        private readonly IContractService _contractService;
        private readonly ISecurityService _securityService;
        public ContractController(ILogService logService, IOptions<ConfigurationMessages> messagesDefault
          , ISecurityService securityService, IContractService contractService)
        {
            _logService = logService;
            _securityService = securityService;
            _messagesDefault = messagesDefault.Value;
            _contractService = contractService;
        }

        [Route(nameof(AddContract)), HttpPost]
        public async Task<ResponseAPI<string>> AddContract(Contract request)
        {
            ResponseAPI<string> responseAPI = new();
            try
            {
                _logService.SaveLogApp($"[{nameof(AddContract)} Request] {JsonConvert.SerializeObject(request)}", LogType.Information);
                #region AUTH
                if (!_securityService.ValidAuth(HttpContext.Request.Headers["Key-Auth"]))
                {
                    _logService.SaveLogApp($"[{nameof(AddContract)} InvalidAuth]", LogType.Information);
                    responseAPI.Code = ResponseCode.Error;
                    responseAPI.Description = _messagesDefault.AuthInvalidMessage;
                    return responseAPI;
                }
                #endregion                
                responseAPI = await _contractService.AddContract(request);
                _logService.SaveLogApp($"[{nameof(AddContract)} Response] {JsonConvert.SerializeObject(responseAPI)}", LogType.Information);
            }
            catch (Exception ex)
            {
                responseAPI.Code = ResponseCode.FatalError;
                responseAPI.Description = _messagesDefault.FatalErrorMessage;
                _logService.SaveLogApp($"[{nameof(ContractController)}  {nameof(AddContract)} Exception] {ex.Message}|{ex.StackTrace}", LogType.Error);
            }
            return responseAPI;
        }

        [Route(nameof(GetContracts)), HttpPost]
        public async Task<ResponseAPI<List<Contract>>> GetContracts()
        {
            ResponseAPI<List<Contract>> responseAPI = new();
            try
            {
                _logService.SaveLogApp($"[{nameof(GetContracts)} Request]", LogType.Information);
                #region AUTH
                if (!_securityService.ValidAuth(HttpContext.Request.Headers["Key-Auth"]))
                {
                    _logService.SaveLogApp($"[{nameof(GetContracts)} InvalidAuth]", LogType.Information);
                    responseAPI.Code = ResponseCode.Error;
                    responseAPI.Description = _messagesDefault.AuthInvalidMessage;
                    return responseAPI;
                }
                #endregion                
                responseAPI = await _contractService.GetContracts();
                _logService.SaveLogApp($"[{nameof(GetContracts)} Response] {JsonConvert.SerializeObject(responseAPI)}", LogType.Information);
            }
            catch (Exception ex)
            {
                responseAPI.Code = ResponseCode.FatalError;
                responseAPI.Description = _messagesDefault.FatalErrorMessage;
                _logService.SaveLogApp($"[{nameof(VehicleController)}  {nameof(GetContracts)} Exception] {ex.Message}|{ex.StackTrace}", LogType.Error);
            }
            return responseAPI;
        }

        [Route(nameof(DeleteContract)), HttpPost]
        public async Task<ResponseAPI<string>> DeleteContract(Contract request)
        {
            ResponseAPI<string> responseAPI = new();
            try
            {
                _logService.SaveLogApp($"[{nameof(DeleteContract)} Request] {JsonConvert.SerializeObject(request)}", LogType.Information);
                #region AUTH
                if (!_securityService.ValidAuth(HttpContext.Request.Headers["Key-Auth"]))
                {
                    _logService.SaveLogApp($"[{nameof(DeleteContract)} InvalidAuth]", LogType.Information);
                    responseAPI.Code = ResponseCode.Error;
                    responseAPI.Description = _messagesDefault.AuthInvalidMessage;
                    return responseAPI;
                }
                #endregion                
                responseAPI = await _contractService.DeleteContract(request.ContractId);
                _logService.SaveLogApp($"[{nameof(DeleteContract)} Response] {JsonConvert.SerializeObject(responseAPI)}", LogType.Information);
            }
            catch (Exception ex)
            {
                responseAPI.Code = ResponseCode.FatalError;
                responseAPI.Description = _messagesDefault.FatalErrorMessage;
                _logService.SaveLogApp($"[{nameof(UserController)}  {nameof(DeleteContract)} Exception] {ex.Message}|{ex.StackTrace}", LogType.Error);
            }
            return responseAPI;
        }


        [Route(nameof(GetContract)), HttpPost]
        public async Task<ResponseAPI<Contract>> GetContract(Contract request)
        {
            ResponseAPI<Contract> responseAPI = new();
            try
            {
                _logService.SaveLogApp($"[{nameof(GetContract)} Request] {JsonConvert.SerializeObject(request)}", LogType.Information);
                #region AUTH
                if (!_securityService.ValidAuth(HttpContext.Request.Headers["Key-Auth"]))
                {
                    _logService.SaveLogApp($"[{nameof(GetContract)} InvalidAuth]", LogType.Information);
                    responseAPI.Code = ResponseCode.Error;
                    responseAPI.Description = _messagesDefault.AuthInvalidMessage;
                    return responseAPI;
                }
                #endregion                
                responseAPI = await _contractService.GetContract(request.ContractId);
                _logService.SaveLogApp($"[{nameof(GetContract)} Response] {JsonConvert.SerializeObject(responseAPI)}", LogType.Information);
            }
            catch (Exception ex)
            {
                responseAPI.Code = ResponseCode.FatalError;
                responseAPI.Description = _messagesDefault.FatalErrorMessage;
                _logService.SaveLogApp($"[{nameof(ContractController)}  {nameof(GetContract)} Exception] {ex.Message}|{ex.StackTrace}", LogType.Error);
            }
            return responseAPI;
        }

        [Route(nameof(UpdateContract)), HttpPost]
        public async Task<ResponseAPI<string>> UpdateContract(Contract request)
        {
            ResponseAPI<string> responseAPI = new();
            try
            {
                _logService.SaveLogApp($"[{nameof(UpdateContract)} Request] {JsonConvert.SerializeObject(request)}", LogType.Information);
                #region AUTH
                if (!_securityService.ValidAuth(HttpContext.Request.Headers["Key-Auth"]))
                {
                    _logService.SaveLogApp($"[{nameof(UpdateContract)} InvalidAuth]", LogType.Information);
                    responseAPI.Code = ResponseCode.Error;
                    responseAPI.Description = _messagesDefault.AuthInvalidMessage;
                    return responseAPI;
                }
                #endregion                
                responseAPI = await _contractService.UpdateContract(request);
                _logService.SaveLogApp($"[{nameof(UpdateContract)} Response] {JsonConvert.SerializeObject(responseAPI)}", LogType.Information);
            }
            catch (Exception ex)
            {
                responseAPI.Code = ResponseCode.FatalError;
                responseAPI.Description = _messagesDefault.FatalErrorMessage;
                _logService.SaveLogApp($"[{nameof(ContractController)}  {nameof(UpdateContract)} Exception] {ex.Message}|{ex.StackTrace}", LogType.Error);
            }
            return responseAPI;
        }
    }
}
