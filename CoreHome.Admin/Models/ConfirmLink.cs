namespace CoreHome.Admin.Models
{
    public class ConfirmLink
    {
        /// <summary>
        /// 表单ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 表单模态框Id
        /// </summary>
        public string ModalId => $"modal-{Id}";

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
        public Dictionary<string, string> Paras { get; set; } = [];

        /// <summary>
        /// 提示警告
        /// </summary>
        public string Warning { get; set; }
    }
}
