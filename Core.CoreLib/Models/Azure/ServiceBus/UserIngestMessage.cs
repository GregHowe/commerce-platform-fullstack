
using Core.CoreLib.Models.Azure.ActiveDirectory;

namespace Core.CoreLib.Models.Azure.ServiceBus
{
    public class UserIngestMessage : ServiceBusMessageBase
    {
        public List<ADUser> ADUsers { get; set; }
    }
}