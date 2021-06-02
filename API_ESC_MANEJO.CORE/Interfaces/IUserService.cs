using API_ESC_MANEJO.CORE.Entities;
using API_ESC_MANEJO.CORE.Entities.Response;
using System.Threading.Tasks;

namespace API_ESC_MANEJO.CORE.Interfaces
{
    public interface IUserService
    {
        Task<ResponseAPI<string>> LoginUser(User user);
    }
}
