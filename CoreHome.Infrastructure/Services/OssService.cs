using Aliyun.OSS;
using System.IO;

namespace CoreHome.Infrastructure.Services
{
    public class OssService
    {
        private readonly string endPoint = "https://oss-cn-shenzhen.aliyuncs.com/";
        private readonly string accessKeyId = "LTAI4Ftaw9Exp42cJ14axam5";
        private readonly string accessKeySecret = "wt02aBJuyBiygsT2pK8se5GHTdTMZ5";
        private readonly string bucketName = "lllxy";

        private OssClient client;

        public OssService()
        {
            client = new OssClient(endPoint, accessKeyId, accessKeySecret);
        }

        public string UploadBlogPic(string fileName, Stream stream)
        {
            client.PutObject(bucketName, $"CoreHome/Blogs/" + fileName, stream);

            string str = $"https://lllxy.oss-cn-shenzhen.aliyuncs.com/CoreHome/Blogs/{fileName}";
            return str;
        }

    }
}
