using Aliyun.OSS;
using CoreHome.Infrastructure.Models;
using System.Web;

namespace CoreHome.Infrastructure.Services
{
    public class OssService
    {
        private readonly OssClient client;
        private readonly OssConfig config;

        public OssService(OssConfig config)
        {
            client = new OssClient(config.EndPoint, config.AccessKeyId, config.AccessKeySecret);
            this.config = config;
        }

        public string UploadProjCover(Stream stream)
        {
            string fileName = Guid.NewGuid().ToString() + ".jpg";
            string path = "images/projects/";
            _ = client.PutObject(config.BucketName, Path.Combine(path, fileName), stream);
            return Path.Combine(config.BucketDomainName, path, fileName);
        }

        public string GetAvatar()
        {
            return Path.Combine(config.BucketDomainName, "images/avatar.jpg");
        }

        public void UploadAvatar(Stream stream)
        {
            _ = client.PutObject(config.BucketName, "images/avatar.jpg", stream);
        }

        public string GetBackground()
        {
            return Path.Combine(config.BucketDomainName, "images/background.jpg");
        }

        public void UploadBackground(Stream stream)
        {
            _ = client.PutObject(config.BucketName, "images/background.jpg", stream);
        }

        public string UploadBlogPic(Stream stream)
        {
            string fileName = Guid.NewGuid().ToString() + ".jpg";
            string path = "blogs/";
            _ = client.PutObject(config.BucketName, Path.Combine(path, fileName), stream);
            return Path.Combine(config.BucketDomainName, path, fileName);
        }

        public List<string> GetMusics()
        {
            string path = "musics/";
            ObjectListing listing = client.ListObjects(config.BucketName, path);

            List<string> musics = new();
            foreach (OssObjectSummary item in listing.ObjectSummaries)
            {
                if (item.Key != path)
                {
                    musics.Add(Path.Combine(config.BucketDomainName, HttpUtility.UrlEncode(item.Key)));
                }
            }
            return musics;
        }

    }
}
