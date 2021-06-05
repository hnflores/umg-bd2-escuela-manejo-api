using API_ESC_MANEJO.CORE.CORE.Enumerations;
using API_ESC_MANEJO.CORE.Entities;
using API_ESC_MANEJO.CORE.Entities.Configuration;
using API_ESC_MANEJO.CORE.Entities.Response;
using API_ESC_MANEJO.CORE.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API_ESC_MANEJO.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {        
        private readonly ConfigurationMessages _messagesDefault;
        private readonly ILogService _logService;
        private readonly IVehicleService _vehicleService;
        private readonly ISecurityService _securityService;
        public VehicleController(ILogService logService, IOptions<ConfigurationMessages> messagesDefault
          , ISecurityService securityService, IVehicleService vehicleService)
        {
            _logService = logService;

            _securityService = securityService;
            _messagesDefault = messagesDefault.Value;
            _vehicleService = vehicleService;
        }

        [Route(nameof(GetVehicles)), HttpPost]
        public async Task<ResponseAPI<List<Vehicle>>> GetVehicles()
        {
            ResponseAPI<List<Vehicle>> responseAPI = new();
            try
            {
                _logService.SaveLogApp($"[{nameof(GetVehicles)} Request]", LogType.Information);
                #region AUTH
                if (!_securityService.ValidAuth(HttpContext.Request.Headers["Key-Auth"]))
                {
                    _logService.SaveLogApp($"[{nameof(GetVehicles)} InvalidAuth]", LogType.Information);
                    responseAPI.Code = ResponseCode.Error;
                    responseAPI.Description = _messagesDefault.AuthInvalidMessage;
                    return responseAPI;
                }
                #endregion                
                responseAPI = await _vehicleService.GetVehicles();
                _logService.SaveLogApp($"[{nameof(GetVehicles)} Response] {JsonConvert.SerializeObject(responseAPI)}", LogType.Information);
            }
            catch (Exception ex)
            {
                responseAPI.Code = ResponseCode.FatalError;
                responseAPI.Description = _messagesDefault.FatalErrorMessage;
                _logService.SaveLogApp($"[{nameof(VehicleController)}  {nameof(GetVehicles)} Exception] {ex.Message}|{ex.StackTrace}", LogType.Error);
            }
            return responseAPI;
        }

        [Route(nameof(GetVehicle)), HttpPost]
        public async Task<ResponseAPI<Vehicle>> GetVehicle(Vehicle request)
        {
            ResponseAPI<Vehicle> responseAPI = new();
            try
            {
                _logService.SaveLogApp($"[{nameof(GetVehicle)} Request] {JsonConvert.SerializeObject(request)}", LogType.Information);
                #region AUTH
                if (!_securityService.ValidAuth(HttpContext.Request.Headers["Key-Auth"]))
                {
                    _logService.SaveLogApp($"[{nameof(GetVehicle)} InvalidAuth]", LogType.Information);
                    responseAPI.Code = ResponseCode.Error;
                    responseAPI.Description = _messagesDefault.AuthInvalidMessage;
                    return responseAPI;
                }
                #endregion                
                responseAPI = await _vehicleService.GetVehicle(request.VehicleId);
                _logService.SaveLogApp($"[{nameof(GetVehicle)} Response] {JsonConvert.SerializeObject(responseAPI)}", LogType.Information);
            }
            catch (Exception ex)
            {
                responseAPI.Code = ResponseCode.FatalError;
                responseAPI.Description = _messagesDefault.FatalErrorMessage;
                _logService.SaveLogApp($"[{nameof(UserController)}  {nameof(GetVehicle)} Exception] {ex.Message}|{ex.StackTrace}", LogType.Error);
            }
            return responseAPI;
        }

        [Route(nameof(AddVehicle)), HttpPost]
        public async Task<ResponseAPI<string>> AddVehicle(Vehicle request)
        {
            ResponseAPI<string> responseAPI = new();
            try
            {
                _logService.SaveLogApp($"[{nameof(AddVehicle)} Request] {JsonConvert.SerializeObject(request)}", LogType.Information);
                #region AUTH
                if (!_securityService.ValidAuth(HttpContext.Request.Headers["Key-Auth"]))
                {
                    _logService.SaveLogApp($"[{nameof(AddVehicle)} InvalidAuth]", LogType.Information);
                    responseAPI.Code = ResponseCode.Error;
                    responseAPI.Description = _messagesDefault.AuthInvalidMessage;
                    return responseAPI;
                }
                #endregion                
                responseAPI = await _vehicleService.AddVehicle(request);
                _logService.SaveLogApp($"[{nameof(AddVehicle)} Response] {JsonConvert.SerializeObject(responseAPI)}", LogType.Information);
            }
            catch (Exception ex)
            {
                responseAPI.Code = ResponseCode.FatalError;
                responseAPI.Description = _messagesDefault.FatalErrorMessage;
                _logService.SaveLogApp($"[{nameof(UserController)}  {nameof(AddVehicle)} Exception] {ex.Message}|{ex.StackTrace}", LogType.Error);
            }
            return responseAPI;
        }

        [Route(nameof(UpdateVehicle)), HttpPost]
        public async Task<ResponseAPI<string>> UpdateVehicle(Vehicle request)
        {
            ResponseAPI<string> responseAPI = new();
            try
            {
                _logService.SaveLogApp($"[{nameof(UpdateVehicle)} Request] {JsonConvert.SerializeObject(request)}", LogType.Information);
                #region AUTH
                if (!_securityService.ValidAuth(HttpContext.Request.Headers["Key-Auth"]))
                {
                    _logService.SaveLogApp($"[{nameof(UpdateVehicle)} InvalidAuth]", LogType.Information);
                    responseAPI.Code = ResponseCode.Error;
                    responseAPI.Description = _messagesDefault.AuthInvalidMessage;
                    return responseAPI;
                }
                #endregion                
                responseAPI = await _vehicleService.UpdateVehicle(request);
                _logService.SaveLogApp($"[{nameof(UpdateVehicle)} Response] {JsonConvert.SerializeObject(responseAPI)}", LogType.Information);
            }
            catch (Exception ex)
            {
                responseAPI.Code = ResponseCode.FatalError;
                responseAPI.Description = _messagesDefault.FatalErrorMessage;
                _logService.SaveLogApp($"[{nameof(UserController)}  {nameof(UpdateVehicle)} Exception] {ex.Message}|{ex.StackTrace}", LogType.Error);
            }
            return responseAPI;
        }

        [Route(nameof(DeleteVehicle)), HttpPost]
        public async Task<ResponseAPI<string>> DeleteVehicle(Vehicle request)
        {
            ResponseAPI<string> responseAPI = new();
            try
            {
                _logService.SaveLogApp($"[{nameof(DeleteVehicle)} Request] {JsonConvert.SerializeObject(request)}", LogType.Information);
                #region AUTH
                if (!_securityService.ValidAuth(HttpContext.Request.Headers["Key-Auth"]))
                {
                    _logService.SaveLogApp($"[{nameof(DeleteVehicle)} InvalidAuth]", LogType.Information);
                    responseAPI.Code = ResponseCode.Error;
                    responseAPI.Description = _messagesDefault.AuthInvalidMessage;
                    return responseAPI;
                }
                #endregion                
                responseAPI = await _vehicleService.DeleteVehicle(request.VehicleId);
                _logService.SaveLogApp($"[{nameof(DeleteVehicle)} Response] {JsonConvert.SerializeObject(responseAPI)}", LogType.Information);
            }
            catch (Exception ex)
            {
                responseAPI.Code = ResponseCode.FatalError;
                responseAPI.Description = _messagesDefault.FatalErrorMessage;
                _logService.SaveLogApp($"[{nameof(UserController)}  {nameof(DeleteVehicle)} Exception] {ex.Message}|{ex.StackTrace}", LogType.Error);
            }
            return responseAPI;
        }
    }
}
