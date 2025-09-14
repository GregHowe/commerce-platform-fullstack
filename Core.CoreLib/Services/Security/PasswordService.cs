
using System.Security.Cryptography;
using System.Text;

namespace Core.CoreLib.Services.Security
{
    public class PasswordService : IPasswordService
    {
        // Generated at random (Guid.NewGuid()), once this method of password generation is approved and used with creating users in AD, this should
        // not be changed without a lot of consideration as users generated with this guid as part of the seed will be difficult to
        // figure out their existing password.  If changed, previous instances of this GUID should be documented.
        private const string Seed = "30fc46f8-87f6-407f-8cad-2c19ea4cb1c4";

        public string GeneratePassword(string userEmail) =>
            HashPassword($"{userEmail.Substring(0, userEmail.IndexOf("@"))}-{Seed}");

        private string HashPassword(string password)
        {
            var inputBytes = Encoding.ASCII.GetBytes(password);
            var hash = SHA1.Create().ComputeHash(inputBytes);

            return Convert.ToBase64String(hash);
        }

        public bool VerifyHashedPassword(string hashedPassword, string userEmail)
        {
            var generated = GeneratePassword(userEmail);
            return generated == hashedPassword;
        }
    }
}