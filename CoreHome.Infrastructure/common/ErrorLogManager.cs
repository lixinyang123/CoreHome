using System;
using System.IO;

namespace Infrastructure.common
{
    public static class ErrorLogManager
    {
        private static readonly string path = "C:\\Server";
        private static readonly string errorLog = path + "\\ErrorLog.txt";

        /// <summary>
        /// 记录异常信息
        /// </summary>
        public static void SetErrorLog(Exception ex)
        {
            if (!Directory.Exists(errorLog))
            {
                Directory.CreateDirectory(errorLog);
            }

            if (!File.Exists(errorLog))
            {
                File.Create(errorLog);
            }

            using (StreamWriter sw = File.AppendText(errorLog))
            {
                sw.WriteLine(ex.ToString());
            }
        }
    }
}
