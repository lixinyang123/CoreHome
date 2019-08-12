using System;
using System.IO;
using System.IO.Compression;
using System.Xml;
using coreHome.Models;

namespace coreHome.Servrice
{
    public class BlogFileManager
    {
        private readonly string blogAssetsPath;
        private readonly string blogConfigPath;
        private readonly string tempBlogFile;

        public BlogFileManager(string webRootPath)
        {
            blogAssetsPath = webRootPath + "\\blogassets";
            blogConfigPath = blogAssetsPath + "\\blog\\config.xml";
            tempBlogFile = blogAssetsPath + "\\blog";
        }

        public bool SaveBlogFile(Stream fileStream,out Article article)
        {
            //保存博客文件
            int length = Convert.ToInt32(fileStream.Length);
            byte[] buffer = new byte[length];
            fileStream.Read(buffer, 0, length);

            fileStream.Close();

            string fullPath = blogAssetsPath + "\\temp.zip";
            File.WriteAllBytes(fullPath, buffer);

            if(Directory.Exists(tempBlogFile))
            {
                Directory.Delete(tempBlogFile,true);
            }
            //解压缩博客
            ZipFile.ExtractToDirectory(fullPath, blogAssetsPath);
            //删除压缩包
            File.Delete(fullPath);

            article = GetBlogInfo();

            if (article != null)
            {
                //更改博客文件夹名称
                if (RenameFolder(article.Title))
                {
                    return true;
                }
            }
            //博客文件格式不正确
            NotifyManager.PushNotify("警告", "检测到非法博客文件上传");
            return false;
        }

        /// <summary>
        /// 读取博客配置信息
        /// </summary>
        private Article GetBlogInfo()
        {
            try
            {
                XmlDocument document = new XmlDocument();
                document.Load(blogConfigPath);

                Article article = new Article()
                {
                    Title = document.GetElementsByTagName("title").Item(0).InnerText,
                    Time = document.GetElementsByTagName("time").Item(0).InnerText,
                    Overview = document.GetElementsByTagName("overview").Item(0).InnerText
                };

                return article;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private bool RenameFolder(string blogName)
        {
            if(Directory.Exists(blogAssetsPath + "\\blog"))
            {
                DirectoryInfo dir = Directory.CreateDirectory(blogAssetsPath + "\\blog");
                string newPath = blogAssetsPath + "\\" + blogName;
                if (!Directory.Exists(newPath))
                {
                    dir.MoveTo(newPath);
                    return true;
                }
                dir.Delete(true);
            }
            return false;
        }

    }
}
