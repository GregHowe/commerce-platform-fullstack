using Core.DotnetFunctions.AppSettings;
using System.Threading.Tasks;

namespace Core.DotnetFunctions.Services.UserIngest
{
    public interface IUserIngestService
    {
        Task ProcessUserIngestion();

        Task SendMessageAsync<T>(
            T message,
            string serviceBusConnectionString,
            string topic);
    }
}
