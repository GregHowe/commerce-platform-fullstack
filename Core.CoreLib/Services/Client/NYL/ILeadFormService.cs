
namespace Core.CoreLib.Services.Client.NYL
{
    public interface ILeadFormService
    {
        Task<HttpResponseMessage> SubmitNYLCLTLead(string formData);
    }
}
