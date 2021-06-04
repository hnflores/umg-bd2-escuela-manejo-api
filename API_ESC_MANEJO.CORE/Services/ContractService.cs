using API_ESC_MANEJO.CORE.CORE.Enumerations;
using API_ESC_MANEJO.CORE.Entities;
using API_ESC_MANEJO.CORE.Entities.Configuration;
using API_ESC_MANEJO.CORE.Entities.Response;
using API_ESC_MANEJO.CORE.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_ESC_MANEJO.CORE.Services
{
    public class ContractService : IContractService
    {
        private readonly Dictionary<string, dynamic> _bdParameters;
        private ResponseBD _responseBD;
        private readonly ConfigurationMessages _messagesDefault;
        private readonly ILogService _logService;
        private readonly IAdminRepository _adminRepository;
        private readonly string _sp = "sp_contract";

        public ContractService(ILogService logService, IAdminRepository adminRepository
            , IOptions<ConfigurationMessages> messagesDefault)
        {
            _bdParameters = new Dictionary<string, dynamic>();
            _messagesDefault = messagesDefault.Value;
            _logService = logService;
            _adminRepository = adminRepository;
        }

        public async Task<ResponseAPI<string>> AddContract(Entities.Contract contract)
        {
            ResponseAPI<string> responseAPI = new();
            try
            {
                _bdParameters.Add("@i_operation", "ADD_CONTRACT");
                _bdParameters.Add("@i_vehiculo_id", contract.VehicleId);
                _bdParameters.Add("@i_user_id", contract.UserId);
                _bdParameters.Add("@i_customer_id", contract.CustomerId);
                _bdParameters.Add("@i_numero_sesiones", contract.Sesiones.Count);
                _bdParameters.Add("@i_costo", contract.Costo);
                _bdParameters.Add("@i_fecha_inicio", contract.FechaInicio);
                _bdParameters.Add("@i_fecha_fin", contract.FechaFin);


                _responseBD = await _adminRepository.CallSPData(_sp, _bdParameters);
                responseAPI.Code = _responseBD.Code;
                if (_responseBD.Code != ResponseCode.Success)
                {
                    if (_responseBD.Code == ResponseCode.Error)
                        responseAPI.Description = _responseBD.Description;
                    else if (_responseBD.Code == ResponseCode.FatalError)
                        responseAPI.Description = _messagesDefault.FatalErrorMessage;
                    else if (_responseBD.Code == ResponseCode.Timeout)
                        responseAPI.Description = _messagesDefault.TimeoutMessage;

                    return responseAPI;
                }

                DataRow dr = _responseBD.Data.Rows[0];
                int contractId = Convert.ToInt32(dr["ContratoId"]);
                foreach (Sesion sesion in contract.Sesiones)
                {
                    ResponseAPI<string> responseSession = await AddSessionContract(contractId, sesion);
                    if (responseSession.Code != ResponseCode.Success)
                    {
                        responseAPI.Code = responseSession.Code;
                        responseAPI.Description = responseSession.Description;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                responseAPI.Code = ResponseCode.FatalError;
                _logService.SaveLogApp($"[{nameof(AddContract)} - Exception] {ex.Message} | {ex.StackTrace}", LogType.Error);
                responseAPI.Description = _messagesDefault.FatalErrorMessage;
            }
            return responseAPI;
        }

        public async Task<ResponseAPI<string>> AddSessionContract(int contractId, Sesion sesion)
        {
            ResponseAPI<string> responseAPI = new();
            try
            {
                _bdParameters.Clear();
                _bdParameters.Add("@i_operation", "ADD_SESSION_CONTRACT");
                _bdParameters.Add("@i_contrato_id", contractId);
                _bdParameters.Add("@i_fecha_inicio", sesion.FechaInicio);
                _bdParameters.Add("@i_fecha_fin", sesion.FechaFin);
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
                _logService.SaveLogApp($"[{nameof(AddSessionContract)} - Exception] {ex.Message} | {ex.StackTrace}", LogType.Error);
                responseAPI.Description = _messagesDefault.FatalErrorMessage;
            }
            return responseAPI;
        }

        public async Task<ResponseAPI<List<Entities.Contract>>> GetContracts()
        {
            ResponseAPI<List<Entities.Contract>> responseAPI = new();
            List<Entities.Contract> contracts = new();
            try
            {
                _bdParameters.Add("@i_operation", "GET_CONTRACTS");
                _responseBD = await _adminRepository.CallSPData(_sp, _bdParameters);

                if (_responseBD.Code == ResponseCode.Success)
                {
                    foreach (DataRow dr in _responseBD.Data.Rows)
                    {
                        contracts.Add(new Entities.Contract
                        {
                            ContractId = Convert.ToInt32(dr["ContratoId"]),
                            FechaInicio = Convert.ToDateTime(dr["FechaInicio"]),
                            FechaFin = Convert.ToDateTime(dr["FechaFin"]),
                            NumeroSesiones = Convert.ToInt32(dr["NumSesion"]),
                            Costo = Convert.ToDecimal(dr["CostoTotal"]),                            
                            Estado = Convert.ToString(dr["Estado"]),
                            Conductor = Convert.ToString(dr["Conductor"]),
                            Vehiculo = Convert.ToString(dr["Vehiculo"]),
                            Cliente = Convert.ToString(dr["Cliente"])
                        });
                    }
                    responseAPI.Data = contracts;
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
                _logService.SaveLogApp($"[{nameof(GetContracts)} - Exception] {ex.Message} | {ex.StackTrace}", LogType.Error);
                responseAPI.Description = _messagesDefault.FatalErrorMessage;
            }
            return responseAPI;
        }
    }
}
