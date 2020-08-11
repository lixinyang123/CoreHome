using CoreHome.Infrastructure.Models;

namespace CoreHome.Infrastructure.Services
{
    public class UserInfoService : StaticConfig<Profile>
    {
        public UserInfoService(string fileName, Profile initUserInfo) : base(fileName, initUserInfo) { }
    }
}
