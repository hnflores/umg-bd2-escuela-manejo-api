using API_ESC_MANEJO.CORE.CORE.Enumerations;

namespace API_ESC_MANEJO.CORE.Interfaces
{
    public interface ILogService
    {
         void SaveLogApp(string message, LogType logType);
    }
}
