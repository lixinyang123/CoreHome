using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace CoreHome.Infrastructure.Services
{
    public class VerificationCodeService
    {
        public string VerificationCode { get; set; }
        public byte[] VerificationImage { get; set; }

        private readonly FontFamily fontFamily;

        public VerificationCodeService()
        {
            fontFamily = new FontCollection().Add("./wwwroot/font/Ubuntu-BI.ttf");
            CreateCode();
            CreateImage();
        }

        private void CreateCode()
        {
            VerificationCode = Guid.NewGuid().ToString()[..5].ToLower();
        }

        // 随机生成干扰线
        private static List<PointF> RandomPoint()
        {
            Random random = new();
            List<PointF> points = [];
            for (int i = 0; i < 6; i++)
            {
                points.Add(new PointF(random.Next(0, 100), random.Next(0, 42)));
            }
            return points;
        }

        private void CreateImage()
        {
            using Image<Rgba32> image = new(100, 42);

            Pen pen = Pens.Solid(Color.Black, 2);
            Font font = new(fontFamily, 30, FontStyle.Bold);

            // 画验证码
            image.Mutate(i => i.Fill(Color.White)
                .DrawText(VerificationCode, font, Color.Black, new PointF(12, 5)));

            // 画干扰线
            image.Mutate(i => i.DrawLine(pen, RandomPoint().ToArray()));

            MemoryStream ms = new();
            image.Save(ms, new PngEncoder());
            VerificationImage = ms.GetBuffer();
        }

        // System.Drawing.Common 从 .NET 6 开始仅支持 Windows 系统
        // https://www.lllxy.net/Blog/Detail/634b2769-5046-45eb-b71b-fe2a87b7c1fe

        //#pragma warning disable CA1416 // Validate platform compatibility
        //        private void CreateImage()
        //        {
        //            //设置图片大小
        //            Image image = new Bitmap(100, 42);
        //            //设置画笔在哪一张图片上画图
        //            Graphics graph = Graphics.FromImage(image);
        //            //背景色
        //            graph.Clear(Color.White);
        //            //笔刷
        //            Pen pen = new Pen(Brushes.Black, 2);
        //            //干扰线
        //            for (int i = 0; i < 4; i++)
        //            {
        //                int[] points = RandomPoint();
        //                graph.DrawCurve(pen, new Point[] {
        //                    new Point(points[0], points[1]),
        //                    new Point(points[2], points[3]),
        //                    new Point(points[4], points[5])
        //                });
        //            }
        //            //画验证码
        //            graph.DrawString(VerificationCode, new Font(new FontFamily("Microsoft YaHei"), 20, FontStyle.Bold), Brushes.Black, new PointF(10, 0));
        //            //内存流
        //            MemoryStream ms = new MemoryStream();
        //            //把图片存进内存流
        //            image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
        //            //获取内存流的byte数组
        //            VerificationImage = ms.GetBuffer();
        //        }
        //#pragma warning restore CA1416 // Validate platform compatibility
    }
}
