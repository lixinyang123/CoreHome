using Aliyun.OSS;
using CoreHome.Infrastructure.Models;
using System.Collections.Generic;
using System.IO;

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

        public string UploadProjCover(string fileName, Stream stream)
        {
            string path = "images/projects/";
            client.PutObject(config.BucketName, path + fileName, stream);
            return config.BucketDomainName + path + fileName;
        }

        public string UploadAvatar(Stream stream)
        {
            string path = "images/";
            client.PutObject(config.BucketName, path + "avatar.jpg", stream);
            return config.BucketDomainName + path + "avatar.jpg";
        }

        public string UploadBlogPic(string fileName, Stream stream)
        {
            string path = "blogs/";
            client.PutObject(config.BucketName, path + fileName, stream);
            return config.BucketDomainName + path + fileName;
        }

        public List<string> GetMusics()
        {
            string path = "musics/";
            ObjectListing listing = client.ListObjects(config.BucketName, path);

            List<string> musics = new List<string>();
            foreach (OssObjectSummary item in listing.ObjectSummaries)
            {
                if (item.Key != path)
                {
                    musics.Add(config.BucketDomainName + item.Key);
                }
            }
            return musics;
        }

    }
}
