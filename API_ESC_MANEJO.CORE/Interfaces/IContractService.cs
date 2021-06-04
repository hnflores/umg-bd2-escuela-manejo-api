using API_ESC_MANEJO.CORE.Entities.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API_ESC_MANEJO.CORE.Interfaces
{
    public interface IContractService
    {
        Task<ResponseAPI<string>> AddContract(Entities.Contract contract);
        Task<ResponseAPI<List<Entities.Contract>>> GetContracts();
    }
}
