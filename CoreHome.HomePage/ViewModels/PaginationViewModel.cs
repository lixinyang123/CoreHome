namespace CoreHome.HomePage.ViewModels
{
    public class PaginationViewModel
    {
        public int CurrentIndex { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount { get; set; }

        public int MaxLength { get; set; }
    }
}
