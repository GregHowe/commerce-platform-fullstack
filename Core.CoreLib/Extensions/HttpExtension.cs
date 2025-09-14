
namespace Core.CoreLib.Extensions
{
    public static class HttpExtension
    {
        public async static Task<string> ContentToStringAsync(this HttpContent httpContent) =>
            await httpContent.ReadAsStringAsync();
    }
}