using System.Data;

namespace API_ESC_MANEJO.CORE.Entities.Response
{
    public class ResponseBD : BaseResponse
    {
        public DataTable Data { get; set; }
    }
}
