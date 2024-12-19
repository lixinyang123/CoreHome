using CoreHome.Infrastructure.Models;

namespace CoreHome.Infrastructure.Services
{
    public class ProfileService(string fileName, Profile initUserInfo) : StaticConfig<Profile>(fileName, initUserInfo)
    {
    }
}
