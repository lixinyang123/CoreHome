
namespace coreHome.Servrice
{
    public class ArticleManager
    {
        public string GetCoverPath(string title)
        {
            string path = string.Format($"/BlogAssets/{title}/images/cover.png");

            return path;
        }

    }
}
