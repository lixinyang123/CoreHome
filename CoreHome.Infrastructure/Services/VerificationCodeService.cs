using System.Drawing;

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
            VerificationCode = Guid.NewGuid().ToString().Substring(0, 5).ToLower();
        }

        //生成随机点
        private static int[] RandomPoint()
        {
            int[] intArray = new int[6];
            for (int i = 0; i < 6; i += 2)
            {
                Random random = new Random(Guid.NewGuid().GetHashCode());
                switch (i)
                {
                    case 0:
                        intArray[i] = random.Next(0, 10);
                        break;
                    case 2:
                        intArray[i] = random.Next(45, 55);
                        break;
                    case 4:
                        intArray[i] = random.Next(90, 100);
                        break;
                }
            }
            for (int i = 1; i < 6; i += 2)
            {
                Random random = new Random(Guid.NewGuid().GetHashCode());
                intArray[i] = random.Next(0, 42);
            }
            return intArray;
        }

        private void CreateImage()
        {
            //设置图片大小
            Image image = new Bitmap(100, 42);
            //设置画笔在哪一张图片上画图
            Graphics graph = Graphics.FromImage(image);
            //背景色
            graph.Clear(Color.White);
            //笔刷
            Pen pen = new Pen(Brushes.Black, 2);
            for (int i = 0; i < 4; i++)
            {
                int[] points = RandomPoint();
                //画一条曲线
                graph.DrawCurve(pen, new Point[] {
                    new Point(points[0], points[1]),
                    new Point(points[2], points[3]),
                    new Point(points[4], points[5])
                });
            }
            //画一条直线
            //graph.DrawLines(pen, new Point[] { new Point(10, 10), new Point(90, 40) });
            //画数字
            graph.DrawString(VerificationCode, new Font(new FontFamily("Microsoft YaHei"), 20, FontStyle.Bold),
                Brushes.Black, new PointF(10, 0));
            //内存流
            MemoryStream ms = new MemoryStream();
            //把图片存进内存流
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            //获取内存流的byte数组
            VerificationImage = ms.GetBuffer();
        }

    }
}
