using Aliyun.OSS;
using CoreHome.Infrastructure.Models;
using System.IO;

namespace CoreHome.Infrastructure.Services
{
    public class OssService
    {
        private readonly OssClient client;
        private readonly string bucketName;

        public OssService(OssConfig config)
        {
            client = new OssClient(config.EndPoint, config.AccessKeyId, config.AccessKeySecret);
            bucketName = config.BucketName;
        }

        public string UploadBlogPic(string fileName, Stream stream)
        {
            client.PutObject(bucketName, $"CoreHome/Blogs/" + fileName, stream);
            return $"https://lllxy.oss-cn-shenzhen.aliyuncs.com/CoreHome/Blogs/{fileName}";
        }

    }
}
