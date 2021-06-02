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
    public class UserController : ControllerBase
    {
        private readonly ConfigurationMessages _messagesDefault;
        private readonly ILogService _logService;
        private readonly IUserService _userService;
        private readonly ISecurityService _securityService;
        public UserController(ILogService logService, IUserService userService, IOptions<ConfigurationMessages> messagesDefault
          , ISecurityService securityService)
        {
            _logService = logService;
            _userService = userService;
            _securityService = securityService;
            _messagesDefault = messagesDefault.Value;
        }

        [Route(nameof(LogIn)), HttpPost]
        public async Task<ResponseAPI<string>> LogIn(User request)
        {
            ResponseAPI<string> responseAPI = new();
            try
            {
                _logService.SaveLogApp($"[{nameof(LogIn)} Request] {JsonConvert.SerializeObject(request)}", LogType.Information);
                #region AUTH
                if (!_securityService.ValidAuth(HttpContext.Request.Headers["Key-Auth"]))
                {
                    _logService.SaveLogApp($"[{nameof(LogIn)} InvalidAuth]", LogType.Information);
                    responseAPI.Code = ResponseCode.Error;
                    responseAPI.Description = _messagesDefault.AuthInvalidMessage;
                    return responseAPI;
                }
                #endregion                
                responseAPI = await _userService.LoginUser(request);
                _logService.SaveLogApp($"[{nameof(LogIn)} Response] {JsonConvert.SerializeObject(responseAPI)}", LogType.Information);
            }
            catch (Exception ex)
            {
                responseAPI.Code = ResponseCode.FatalError;
                responseAPI.Description = _messagesDefault.FatalErrorMessage;
                _logService.SaveLogApp($"[{nameof(UserController)}  {nameof(LogIn)} Exception] {ex.Message}|{ex.StackTrace}", LogType.Error);
            }
            return responseAPI;
        }
    }
}
