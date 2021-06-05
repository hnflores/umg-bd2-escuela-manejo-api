using API_ESC_MANEJO.CORE.CORE.Enumerations;
using API_ESC_MANEJO.CORE.Entities;
using API_ESC_MANEJO.CORE.Entities.Configuration;
using API_ESC_MANEJO.CORE.Entities.Response;
using API_ESC_MANEJO.CORE.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace API_ESC_MANEJO.CORE.Services
{
    public class UserService : IUserService
    {
        private readonly Dictionary<string, dynamic> _bdParameters;
        private ResponseBD _responseBD;
        private readonly ConfigurationMessages _messagesDefault;
        private readonly ILogService _logService;
        private readonly IAdminRepository _adminRepository;
        private readonly string _sp = "sp_user";

        public UserService(ILogService logService, IAdminRepository adminRepository
            , IOptions<ConfigurationMessages> messagesDefault)
        {
            _bdParameters = new Dictionary<string, dynamic>();
            _messagesDefault = messagesDefault.Value;
            _logService = logService;
            _adminRepository = adminRepository;
        }


        public async Task<ResponseAPI<string>> LoginUser(User user)
        {
            ResponseAPI<string> responseAPI = new();
            try
            {
                _bdParameters.Add("@i_operation", "LOGIN");
                _bdParameters.Add("@i_user", user.Correo);
                _bdParameters.Add("@i_password", user.Password);

                _responseBD = await _adminRepository.CallSP(_sp, _bdParameters);
                responseAPI.Code = _responseBD.Code;
                if (_responseBD.Code != ResponseCode.Success)
                {
                    if (_responseBD.Code == ResponseCode.Error)
                        responseAPI.Description = _responseBD.Description;
                    else if (_responseBD.Code == ResponseCode.FatalError)
                        responseAPI.Description = _messagesDefault.FatalErrorMessage;
                    else if (_responseBD.Code == ResponseCode.Timeout)
                        responseAPI.Description = _messagesDefault.TimeoutMessage;
                }
            }
            catch (Exception ex)
            {
                responseAPI.Code = ResponseCode.FatalError;
                _logService.SaveLogApp($"[{nameof(LoginUser)} - Exception] {ex.Message} | {ex.StackTrace}", LogType.Error);
                responseAPI.Description = _messagesDefault.FatalErrorMessage;
            }
            return responseAPI;
        }

        public async Task<ResponseAPI<List<User>>> GetDrivers()
        {
            ResponseAPI<List<User>> responseAPI = new();
            List<User> users = new();
            try
            {
                _bdParameters.Add("@i_operation", "GET_DRIVERS");
                _responseBD = await _adminRepository.CallSPData(_sp, _bdParameters);

                if (_responseBD.Code == ResponseCode.Success)
                {
                    foreach (DataRow dr in _responseBD.Data.Rows)
                    {
                        users.Add(new User
                        {
                            ColaboradorId = Convert.ToInt32(dr["ColaboradorId"]),
                            Nombre = Convert.ToString(dr["Nombre"]),
                            Apellido = Convert.ToString(dr["Apellido"]),
                            Correo = Convert.ToString(dr["Correo"]),
                            Estado = Convert.ToString(dr["Estado"]),
                            Departamento = Convert.ToString(dr["Departamento"]),
                            Municipio = Convert.ToString(dr["Municipio"]),
                            Colonia = Convert.ToString(dr["Colonia"])
                        });
                    }
                    responseAPI.Data = users;
                    responseAPI.Code = ResponseCode.Success;
                }
                else
                {
                    responseAPI.Code = _responseBD.Code;
                    if (_responseBD.Code == ResponseCode.Error)
                        responseAPI.Description = _responseBD.Description;
                    else if (_responseBD.Code == ResponseCode.FatalError)
                        responseAPI.Description = _messagesDefault.FatalErrorMessage;
                    else if (_responseBD.Code == ResponseCode.Timeout)
                        responseAPI.Description = _messagesDefault.TimeoutMessage;
                }

            }
            catch (Exception ex)
            {
                responseAPI.Code = ResponseCode.FatalError;
                _logService.SaveLogApp($"[{nameof(GetDrivers)} - Exception] {ex.Message} | {ex.StackTrace}", LogType.Error);
                responseAPI.Description = _messagesDefault.FatalErrorMessage;
            }
            return responseAPI;
        }

    }
}
