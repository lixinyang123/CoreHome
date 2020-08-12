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
        private readonly string bucketName = "corehome";

        public OssService(OssConfig config)
        {
            client = new OssClient(config.EndPoint, config.AccessKeyId, config.AccessKeySecret);
            this.config = config;
        }

        public string UploadProjectCover(string fileName, Stream stream)
        {
            string path = "images/";
            client.PutObject(bucketName, path + fileName, stream);
            return config.BucketDomain + path + fileName;
        }

        public string UploadAvatar(Stream stream)
        {
            string path = "images/";
            client.PutObject(bucketName, path + "avatar.jpg", stream);
            return config.BucketDomain + path + "avatar.jpg";
        }

        public string UploadBlogPic(string fileName, Stream stream)
        {
            string path = "blogs/";
            client.PutObject(bucketName, path + fileName, stream);
            return config.BucketDomain + path + fileName;
        }

        public List<string> GetMusics()
        {
            string path = "musics/";
            ObjectListing listing = client.ListObjects(bucketName, path);

            List<string> musics = new List<string>();
            foreach (OssObjectSummary item in listing.ObjectSummaries)
            {
                if (item.Key != path)
                {
                    musics.Add(config.BucketDomain + item.Key);
                }
            }
            return musics;
        }

    }
}
