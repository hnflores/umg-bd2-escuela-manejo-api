using API_ESC_MANEJO.CORE.Entities.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API_ESC_MANEJO.CORE.Interfaces
{
    public interface IAdminRepository
    {
        Task<ResponseBD> CallSP(string sp, Dictionary<string, dynamic> parameters);
        Task<ResponseBD> CallSPData(string sp, Dictionary<string, dynamic> parameters);
    }
}
