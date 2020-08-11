using CoreHome.Infrastructure.Models;

namespace CoreHome.Infrastructure.Services
{
    public class ProfileService : StaticConfig<Profile>
    {
        public ProfileService(string fileName, Profile initUserInfo) : base(fileName, initUserInfo) { }
    }
}
