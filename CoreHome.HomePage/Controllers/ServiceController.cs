using CoreHome.Data.DatabaseContext;
using CoreHome.Data.Models;
using CoreHome.HomePage.ViewModels;
using CoreHome.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoreHome.HomePage.Controllers
{
    public class ServiceController(VerificationCodeService verificationHelper,
        OssService ossService,
        ArticleDbContext articleDbContext,
        IServiceProvider serviceProvider) : Controller
    {
        private readonly VerificationCodeService verificationHelper = verificationHelper;
        private readonly OssService ossService = ossService;
        private readonly IServiceProvider serviceProvider = serviceProvider;
        private readonly ArticleDbContext articleDbContext = articleDbContext;

        /// <summary>
        /// 验证码
        /// </summary>
        /// <returns>验证码图片</returns>
        public IActionResult VerificationCode()
        {
            ISession session = HttpContext.Session;
            session.SetString("VerificationCode", verificationHelper.VerificationCode.ToLower());
            return File(verificationHelper.VerificationImage, "image/png");
        }

        /// <summary>
        /// 随机背景音乐
        /// </summary>
        /// <returns>背景音乐链接</returns>
        public IActionResult BackgroundMusic()
        {
            string musicUrl = serviceProvider.GetService<ThemeService>().Config.MusicUrl;

            if (!string.IsNullOrEmpty(musicUrl))
            {
                return Redirect(musicUrl);
            }

            List<string> musics = ossService.GetMusics();
            return musics.Count == 0 ? NoContent() : Redirect(musics[new Random().Next(musics.Count)]);
        }

        /// <summary>
        /// 搜索提示
        /// </summary>
        /// <param name="id">搜索关键词</param>
        /// <returns>文章提示</returns>
        public async Task<IActionResult> PreSearch(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Json(Array.Empty<object>());
            }

            List<Article> articles = await articleDbContext.Articles
                .AsNoTracking()
                .OrderByDescending(i => i.Id)
                .Where(i => i.Title.Contains(id, StringComparison.CurrentCultureIgnoreCase) || i.Overview.Contains(id, StringComparison.CurrentCultureIgnoreCase))
                .Take(5)
                .ToListAsync();

            List<PreSearchViewModel> viewModels = [];

            articles.ForEach(i =>
            {
                viewModels.Add(new PreSearchViewModel(i.ArticleCode, i.Title, i.Overview));
            });

            return Json(viewModels);
        }
    }
}
