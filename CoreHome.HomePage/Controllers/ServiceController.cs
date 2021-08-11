using CoreHome.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoreHome.HomePage.Controllers
{
    public class ServiceController : Controller
    {
        private readonly VerificationCodeService verificationHelper;
        private readonly OssService ossService;
        private readonly IServiceProvider serviceProvider;

        public ServiceController(VerificationCodeService verificationHelper,
            OssService ossService,
            IServiceProvider serviceProvider)
        {
            this.verificationHelper = verificationHelper;
            this.ossService = ossService;
            this.serviceProvider = serviceProvider;
        }

        public IActionResult VerificationCode()
        {
            ISession session = HttpContext.Session;
            session.SetString("VerificationCode", verificationHelper.VerificationCode.ToLower());
            return File(verificationHelper.VerificationImage, "image/png");
        }

        public IActionResult BackgroundMusic()
        {
            string musicUrl = serviceProvider.GetService<ThemeService>().Config.MusicUrl;

            if (!string.IsNullOrEmpty(musicUrl))
            {
                return Redirect(musicUrl);
            }

            List<string> musics = ossService.GetMusics();
            if (musics.Count == 0)
            {
                return NoContent();
            }

            return Redirect(musics[new Random().Next(musics.Count)]);
        }
    }
}
