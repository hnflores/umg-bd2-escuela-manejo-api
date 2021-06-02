namespace API_ESC_MANEJO.CORE.Entities.Response
{
    public class ResponseAPI<T> : BaseResponse
    {
        public T Data { get; set; }
    }
}
