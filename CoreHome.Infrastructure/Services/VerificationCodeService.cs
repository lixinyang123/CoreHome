using ImageMagick;
using ImageMagick.Drawing;

namespace CoreHome.Infrastructure.Services
{
    public class VerificationCodeService
    {
        public string VerificationCode { get; set; }
        public byte[] VerificationImage { get; set; }

        public VerificationCodeService()
        {
            CreateCode();
            CreateImage();
        }

        private void CreateCode()
        {
            VerificationCode = Guid.NewGuid().ToString()[..5].ToLower();
        }

        // 随机生成干扰线
        private static List<PointD> RandomPoint()
        {
            Random random = new();
            List<PointD> points = [];
            for (int i = 0; i < 6; i++)
            {
                points.Add(new PointD(random.Next(0, 100), random.Next(0, 42)));
            }
            return points;
        }

        private void CreateImage()
        {
            using MagickImage image = new(MagickColors.White, 100, 42);
            IDrawables<byte> drawables = new Drawables()
                .Font("./wwwroot/font/Ubuntu-BI.ttf")
                .FontPointSize(30)
                .FillColor(MagickColors.Black)
                .Text(12, 32, VerificationCode)
                .StrokeColor(MagickColors.Black)
                .StrokeWidth(2)
                .Polyline(RandomPoint());
            _ = drawables.Draw(image);

            using MemoryStream ms = new();
            image.Write(ms, MagickFormat.Png);
            VerificationImage = ms.ToArray();
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
