
namespace Core.CoreLib.Services.Security
{
    public interface IPasswordService
    {
        string GeneratePassword(string userEmail);

        bool VerifyHashedPassword(string hashedPassword, string userEmail);
    }
}
