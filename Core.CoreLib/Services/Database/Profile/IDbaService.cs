using Core.CoreLib.Models.DTO.Profile;
using DbaObject = Core.CoreLib.Models.Database.Core.Dba;

namespace Core.CoreLib.Services.Database.Profile
{
	public interface IDbaService
	{
        public Task<string> InsertDba(SubmitDbaDTO dbaData, string dbaLogoGuid);
        public Task<string> UpdateDba(SubmitDbaDTO dbaData, string dbaLogoGuid);
        public Task<DbaObject> GetDbaUrlGuid(string urlGuid);
    }
}