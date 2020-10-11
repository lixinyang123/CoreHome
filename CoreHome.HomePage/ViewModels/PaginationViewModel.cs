namespace CoreHome.HomePage.ViewModels
{
    public class PaginationViewModel
    {
        public int CurrentIndex { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount { get; set; }

        public string ActionName { get; set; }
    }
}
