using Aliyun.OSS;
using CoreHome.Infrastructure.Models;
using System;
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

        public string UploadProjCover(Stream stream)
        {
            string fileName = Guid.NewGuid().ToString() + ".jpg";
            string path = "images/projects/";
            client.PutObject(config.BucketName, path + fileName, stream);
            return config.BucketDomainName + path + fileName;
        }

        public string GetAvatar()
        {
            return config.BucketDomainName + "images/avatar.jpg";
        }

        public void UploadAvatar(Stream stream)
        {
            client.PutObject(config.BucketName, "images/avatar.jpg", stream);
        }

        public string GetBackground()
        {
            return config.BucketDomainName + "images/background.jpg";
        }

        public void UploadBackground(Stream stream)
        {
            client.PutObject(config.BucketName, "images/background.jpg", stream);
        }

        public string UploadBlogPic(Stream stream)
        {
            string fileName = Guid.NewGuid().ToString() + ".jpg";
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
