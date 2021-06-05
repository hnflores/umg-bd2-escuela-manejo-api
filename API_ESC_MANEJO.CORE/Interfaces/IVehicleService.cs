using API_ESC_MANEJO.CORE.Entities;
using API_ESC_MANEJO.CORE.Entities.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API_ESC_MANEJO.CORE.Interfaces
{
    public interface IVehicleService
    {
        Task<ResponseAPI<List<Vehicle>>> GetVehicles();
        Task<ResponseAPI<Vehicle>> GetVehicle(string vehicleId);
        Task<ResponseAPI<string>> AddVehicle(Vehicle vehicle);
        Task<ResponseAPI<string>> UpdateVehicle(Vehicle vehicle);
        Task<ResponseAPI<string>> DeleteVehicle(string vehicleId);
    }
}
