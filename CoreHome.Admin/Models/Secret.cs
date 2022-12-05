using MemoryPack;

namespace CoreHome.Admin.Models
{
    [MemoryPackable]
    public partial class Secret
    {
        // 初始化向量
        public string IV { get; set; }

        // 密钥
        public string Key { get; set; }
    }
}