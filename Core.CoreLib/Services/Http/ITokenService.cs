
namespace Core.CoreLib.Services.Http
{
	public interface ITokenService
	{
        public Task<string> GenerateBearerTokenAsync(Dictionary<string, string> content, string tokenUrl);
    }
}