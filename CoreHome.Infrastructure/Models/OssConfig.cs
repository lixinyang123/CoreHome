using MemoryPack;

namespace CoreHome.Infrastructure.Models
{
    [MemoryPackable]
    public partial class OssConfig
    {
        public string AccessKeyId { get; set; }

        public string AccessKeySecret { get; set; }

        public string EndPoint { get; set; }

        public string BucketDomainName { get; set; }

        public string BucketName { get; set; }
    }
}
