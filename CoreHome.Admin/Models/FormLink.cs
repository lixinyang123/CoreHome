using System.Collections.Generic;

namespace CoreHome.Admin.Models
{
    public class FormLink
    {
        public FormLink()
        {
            Paras = new Dictionary<string, string>();
        }

        /// <summary>
        /// POST地址
        /// </summary>
        public string Href { get; set; }

        /// <summary>
        /// 链接显示文本
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// CSS类名
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// Post参数
        /// </summary>
        public Dictionary<string, string> Paras { get; set; }

        /// <summary>
        /// 提示警告
        /// </summary>
        public string Warning { get; set; }
    }
}
