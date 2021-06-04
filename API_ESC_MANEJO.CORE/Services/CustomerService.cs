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
    public class CustomerService : ICustomerService
    {
        private readonly Dictionary<string, dynamic> _bdParameters;
        private ResponseBD _responseBD;
        private readonly ConfigurationMessages _messagesDefault;
        private readonly ILogService _logService;
        private readonly IAdminRepository _adminRepository;
        private readonly string _sp = "sp_customer";

        public CustomerService(ILogService logService, IAdminRepository adminRepository
            , IOptions<ConfigurationMessages> messagesDefault)
        {
            _bdParameters = new Dictionary<string, dynamic>();
            _messagesDefault = messagesDefault.Value;
            _logService = logService;
            _adminRepository = adminRepository;
        }

        public async Task<ResponseAPI<List<Customer>>> GetCustomers()
        {
            ResponseAPI<List<Customer>> responseAPI = new();
            List<Customer> customers = new();
            try
            {
                _bdParameters.Add("@i_operation", "GET_CUSTOMERS");
                _responseBD = await _adminRepository.CallSPData(_sp, _bdParameters);

                if (_responseBD.Code == ResponseCode.Success)
                {
                    foreach (DataRow dr in _responseBD.Data.Rows)
                    {
                        customers.Add(new Customer
                        {
                            CustomerId = Convert.ToString(dr["ClienteId"]),
                            Nombre = Convert.ToString(dr["Nombre"]),
                            Apellido = Convert.ToString(dr["Apellido"]),
                            Telefono = Convert.ToString(dr["Telefono"]),
                            FechaNacimiento = Convert.ToDateTime(dr["FechaNacimiento"]),
                            Correo = Convert.ToString(dr["Correo"]),
                            Estado = Convert.ToString(dr["Estado"])
                        });
                    }
                    responseAPI.Data = customers;
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
                _logService.SaveLogApp($"[{nameof(GetCustomers)} - Exception] {ex.Message} | {ex.StackTrace}", LogType.Error);
                responseAPI.Description = _messagesDefault.FatalErrorMessage;
            }
            return responseAPI;
        }

        public async Task<ResponseAPI<Customer>> GetCustomerById(string customerId)
        {
            ResponseAPI<Customer> responseAPI = new();
            Customer customer = new();
            try
            {
                _bdParameters.Add("@i_operation", "GET_CUSTOMER_BY_ID");
                _bdParameters.Add("@i_vehiculo_id", customerId);
                _responseBD = await _adminRepository.CallSPData(_sp, _bdParameters);

                if (_responseBD.Code == ResponseCode.Success)
                {
                    foreach (DataRow dr in _responseBD.Data.Rows)
                    {
                        if (_responseBD.Data.Rows.Count > 0)
                        {
                            customer.CustomerId = Convert.ToString(dr["ClienteId"]);
                            customer.Nombre = Convert.ToString(dr["Nombre"]);
                            customer.Apellido = Convert.ToString(dr["Apellido"]);
                            customer.Telefono = Convert.ToString(dr["Telefono"]);
                            customer.FechaNacimiento = Convert.ToDateTime(dr["FechaNacimiento"]);
                            customer.Correo = Convert.ToString(dr["Correo"]);
                            customer.Estado = Convert.ToString(dr["Estado"]);
                        }
                    }
                    responseAPI.Data = customer;
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
                _logService.SaveLogApp($"[{nameof(GetCustomerById)} - Exception] {ex.Message} | {ex.StackTrace}", LogType.Error);
                responseAPI.Description = _messagesDefault.FatalErrorMessage;
            }
            return responseAPI;
        }


    }
}
