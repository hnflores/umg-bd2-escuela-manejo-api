using API_ESC_MANEJO.CORE.CORE.Enumerations;
using API_ESC_MANEJO.CORE.Entities.Configuration;
using API_ESC_MANEJO.CORE.Entities.Response;
using API_ESC_MANEJO.CORE.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace API_ESC_MANEJO.INFRASTRUCTURE.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly ResponseBD _responseBD;
        private readonly BDModel _administratorBD;
        //private readonly ICryptoService _cryptoService;
        //private readonly IWritableOptions<ConfigurationBD> _writableConfigurationBD;
        private readonly ILogService _logService;
        private readonly string _connectionString;

        public AdminRepository(IOptions<ConfigurationBD> configurationBD
            , ILogService logService)
        {
            _logService = logService;
            _administratorBD = configurationBD.Value.Administrator;
            //_cryptoService = cryptoService;
            //_writableConfigurationBD = writableConfigurationBD;
            //string passwordBD;
            //if (!_administratorBD.Password.StartsWith("$"))
            //{
            //    _writableConfigurationBD.Update(configuration =>
            //    {
            //        _administratorBD.Password = $"${ _cryptoService.Encode(_administratorBD.Password)}";
            //    });
            //    passwordBD = _administratorBD.Password;
            //}
            //passwordBD = _cryptoService.Decode(_administratorBD.Password.Substring(1, _administratorBD.Password.Length - 1));

            _connectionString = $"Server={_administratorBD.Server};Database={_administratorBD.Name};" +
                                $"User Id={_administratorBD.User};Password={_administratorBD.Password}";

            _responseBD = new ResponseBD();
        }

        private SqlConnection GetSqlConnection()
        {
            return new SqlConnection(_connectionString);
        }
        public async Task<ResponseBD> CallSP(string sp, Dictionary<string, dynamic> parameters)
        {
            SqlConnection sql = GetSqlConnection();
            try
            {
                using SqlCommand cmd = new SqlCommand(sp, sql)
                {
                    CommandType = CommandType.StoredProcedure
                };
                foreach (KeyValuePair<string, dynamic> item in parameters)
                {
                    cmd.Parameters.AddWithValue(item.Key, item.Value);
                }
                await sql.OpenAsync();

                await cmd.ExecuteNonQueryAsync();

                _responseBD.Code = ResponseCode.Success;
                return _responseBD;
            }
            catch (SqlException ex)
            {
                _responseBD.Code = ResponseCode.Error;
                _responseBD.Description = ex.Message;

                _logService.SaveLogApp($"{nameof(CallSP)}{nameof(SqlException)} {ex.Message}", LogType.Information);
                return _responseBD;
            }
            catch (TimeoutException ex)
            {
                _responseBD.Code = ResponseCode.Timeout;
                _responseBD.Description = ex.Message;
                _logService.SaveLogApp($"{nameof(CallSP)}{nameof(TimeoutException)} {ex.Message}", LogType.Information);

                return _responseBD;
            }
            catch (Exception ex)
            {
                _responseBD.Code = ResponseCode.FatalError;
                _responseBD.Description = ex.Message;
                _logService.SaveLogApp($"{nameof(CallSP)}{nameof(Exception)} {ex.Message} | {ex.StackTrace}", LogType.Error);
                return _responseBD;
            }
            finally
            {
                await sql.CloseAsync();
            }

        }
        public async Task<ResponseBD> CallSPData(string sp, Dictionary<string, dynamic> parameters)
        {
            DataTable dt = new();
            SqlConnection sql = GetSqlConnection();
            try
            {
                using SqlCommand cmd = new(sp, sql)
                {
                    CommandType = CommandType.StoredProcedure
                };

                foreach (KeyValuePair<string, dynamic> item in parameters)
                {
                    cmd.Parameters.AddWithValue(item.Key, item.Value);
                }
                await sql.OpenAsync();

                var reader = await cmd.ExecuteReaderAsync();
                dt.Load(reader); sql.Close(); reader.Close(); cmd.Dispose(); sql.Dispose();

                _responseBD.Code = ResponseCode.Success;
                _responseBD.Data = dt;
                return _responseBD;
            }
            catch (SqlException ex)
            {
                _responseBD.Code = ResponseCode.Error;
                _responseBD.Description = ex.Message;
                _logService.SaveLogApp($"{nameof(CallSPData)}{nameof(SqlException)} {ex.Message}", LogType.Information);
                return _responseBD;
            }
            catch (TimeoutException ex)
            {
                _responseBD.Code = ResponseCode.Timeout;
                _responseBD.Description = ex.Message;
                _logService.SaveLogApp($"{nameof(CallSPData)}{nameof(TimeoutException)} {ex.Message}", LogType.Information);
                return _responseBD;
            }
            catch (Exception ex)
            {
                _responseBD.Code = ResponseCode.FatalError;
                _responseBD.Description = ex.Message;
                _logService.SaveLogApp($"{nameof(CallSPData)}{nameof(Exception)} {ex.Message} | {ex.StackTrace}", LogType.Error);
                return _responseBD;
            }
            finally
            {
                await sql.CloseAsync();
            }

        }
    }
}
