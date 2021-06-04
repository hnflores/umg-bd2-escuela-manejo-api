using API_ESC_MANEJO.CORE.Entities;
using API_ESC_MANEJO.CORE.Entities.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API_ESC_MANEJO.CORE.Interfaces
{
    public interface ICustomerService
    {
        Task<ResponseAPI<List<Customer>>> GetCustomers();
        Task<ResponseAPI<Customer>> GetCustomerById(string customerId);
    }
}
