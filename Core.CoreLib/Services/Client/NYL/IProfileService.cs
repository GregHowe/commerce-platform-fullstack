
using Core.CoreLib.Models.DTO.Profile;

namespace Core.CoreLib.Services.Client.NYL
{
	public interface IProfileService
    {
        public Task<HttpResponseMessage> SubmitDba(SubmitDbaDTO dbaData);
        public Task<HttpResponseMessage> UpdateDba(SubmitDbaDTO dbaData);
    }
}