using API_ESC_MANEJO.CORE.CORE.Enumerations;
using API_ESC_MANEJO.CORE.Entities;
using API_ESC_MANEJO.CORE.Entities.Configuration;
using API_ESC_MANEJO.CORE.Entities.Response;
using API_ESC_MANEJO.CORE.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_ESC_MANEJO.CORE.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly Dictionary<string, dynamic> _bdParameters;
        private ResponseBD _responseBD;
        private readonly ConfigurationMessages _messagesDefault;
        private readonly ILogService _logService;
        private readonly IAdminRepository _adminRepository;
        private readonly string _sp = "sp_vehicle";
        public VehicleService(ILogService logService, IAdminRepository adminRepository
            , IOptions<ConfigurationMessages> messagesDefault)
        {
            _bdParameters = new Dictionary<string, dynamic>();
            _messagesDefault = messagesDefault.Value;
            _logService = logService;
            _adminRepository = adminRepository;
        }

        public async Task<ResponseAPI<List<Vehicle>>> GetVehicles()
        {
            ResponseAPI<List<Vehicle>> responseAPI = new();
            List<Vehicle> vehicles = new();
            try
            {
                _bdParameters.Add("@i_operation", "GET_VEHICLES");
                _responseBD = await _adminRepository.CallSPData(_sp, _bdParameters);

                if (_responseBD.Code == ResponseCode.Success)
                {
                    foreach (DataRow dr in _responseBD.Data.Rows)
                    {
                        vehicles.Add(new Vehicle
                        {
                            VehicleId = Convert.ToString(dr["VehiculoId"]),
                            Marca = Convert.ToString(dr["Marca"]),
                            Modelo = Convert.ToString(dr["Modelo"]),
                            Anio = Convert.ToString(dr["Anio"]),
                            FechaCompra = Convert.ToDateTime(dr["FechaCompra"]),
                            Estado = Convert.ToString(dr["Estado"])
                        });
                    }
                    responseAPI.Data = vehicles;
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
                _logService.SaveLogApp($"[{nameof(GetVehicles)} - Exception] {ex.Message} | {ex.StackTrace}", LogType.Error);
                responseAPI.Description = _messagesDefault.FatalErrorMessage;
            }
            return responseAPI;
        }

        public async Task<ResponseAPI<Vehicle>> GetVehicle(string vehicleId)
        {
            ResponseAPI<Vehicle> responseAPI = new();
            Vehicle vehicle = new();
            try
            {
                _bdParameters.Add("@i_operation", "GET_VEHICLE");
                _bdParameters.Add("@i_vehiculo_id", vehicleId);
                _responseBD = await _adminRepository.CallSPData(_sp, _bdParameters);

                if (_responseBD.Code == ResponseCode.Success)
                {
                    foreach (DataRow dr in _responseBD.Data.Rows)
                    {
                        if (_responseBD.Data.Rows.Count > 0)
                        {
                            vehicle.VehicleId = Convert.ToString(dr["VehiculoId"]);
                            vehicle.Marca = Convert.ToString(dr["Marca"]);
                            vehicle.Modelo = Convert.ToString(dr["Modelo"]);
                            vehicle.Anio = Convert.ToString(dr["Anio"]);
                            vehicle.FechaCompra = Convert.ToDateTime(dr["FechaCompra"]);
                            vehicle.Estado = Convert.ToString(dr["Estado"]);
                        }
                    }
                    responseAPI.Data = vehicle;
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
                _logService.SaveLogApp($"[{nameof(GetVehicle)} - Exception] {ex.Message} | {ex.StackTrace}", LogType.Error);
                responseAPI.Description = _messagesDefault.FatalErrorMessage;
            }
            return responseAPI;
        }

        public async Task<ResponseAPI<string>> AddVehicle(Vehicle vehicle)
        {
            ResponseAPI<string> responseAPI = new();
            try
            {
                _bdParameters.Add("@i_operation", "ADD_VEHICLE");
                _bdParameters.Add("@i_vehiculo_id", vehicle.VehicleId);
                _bdParameters.Add("@i_marca", vehicle.Marca);
                _bdParameters.Add("@i_modelo", vehicle.Modelo);
                _bdParameters.Add("@i_anio", vehicle.Anio);
                _bdParameters.Add("@i_fecha_compra", vehicle.FechaCompra);

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
                _logService.SaveLogApp($"[{nameof(AddVehicle)} - Exception] {ex.Message} | {ex.StackTrace}", LogType.Error);
                responseAPI.Description = _messagesDefault.FatalErrorMessage;
            }
            return responseAPI;
        }

        public async Task<ResponseAPI<string>> UpdateVehicle(Vehicle vehicle)
        {
            ResponseAPI<string> responseAPI = new();
            try
            {
                _bdParameters.Add("@i_operation", "UPDATE_VEHICLE");
                _bdParameters.Add("@i_vehiculo_id", vehicle.VehicleId);
                _bdParameters.Add("@i_marca", vehicle.Marca);
                _bdParameters.Add("@i_modelo", vehicle.Modelo);
                _bdParameters.Add("@i_anio", vehicle.Anio);
                _bdParameters.Add("@i_fecha_compra", vehicle.FechaCompra);
                _bdParameters.Add("@i_estado", vehicle.Estado);

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
                _logService.SaveLogApp($"[{nameof(AddVehicle)} - Exception] {ex.Message} | {ex.StackTrace}", LogType.Error);
                responseAPI.Description = _messagesDefault.FatalErrorMessage;
            }
            return responseAPI;
        }

        public async Task<ResponseAPI<string>> DeleteVehicle(string vehicleId)
        {
            ResponseAPI<string> responseAPI = new();
            try
            {
                _bdParameters.Add("@i_operation", "DELETE_VEHICLE");
                _bdParameters.Add("@i_vehiculo_id", vehicleId);               

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
                _logService.SaveLogApp($"[{nameof(DeleteVehicle)} - Exception] {ex.Message} | {ex.StackTrace}", LogType.Error);
                responseAPI.Description = _messagesDefault.FatalErrorMessage;
            }
            return responseAPI;
        }


    }
}
