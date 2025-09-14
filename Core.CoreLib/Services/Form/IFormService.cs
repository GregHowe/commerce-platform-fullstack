using Core.CoreLib.Models.Database.Core;

namespace Core.CoreLib.Services.Form
{
    public interface IFormService
    {

        /// <summary>
        /// Save the form
        /// </summary>
        /// <param name="leadForm"></param>
        /// <returns></returns>
        public Task<string> GenerateReferenceNumber(int siteId);
    }
}
