using Core.CoreLib.Models.Tooling;

namespace Core.CoreLib.Services.Tooling
{
    public interface IGRecaptchaService
    {
        public Task<string> SubmitRecaptcha(GRRequest recaptchaReq);
    }
}

