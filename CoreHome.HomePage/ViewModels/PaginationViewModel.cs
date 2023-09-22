namespace CoreHome.HomePage.ViewModels
{
    public class PaginationViewModel
    {
        /// <summary>
        /// 当前索引前后显示的索引数量
        /// 显示索引总数 = preShowNum * 2 + 1
        /// </summary>
        private const int preShowNum = 2;

        /// <summary>
        /// 当前页面索引
        /// </summary>
        public int CurrentIndex { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount { get; set; }

        /// <summary>
        /// 跳转的 action 名称
        /// </summary>
        public string ActionName { get; set; }

        public PaginationViewModel(int currentIndex, int pageCount, string actionName)
        {
            CurrentIndex = currentIndex;
            PageCount = pageCount;
            ActionName = actionName;
        }

        /// <summary>
        /// 显示的索引
        /// </summary>
        public Dictionary<int, bool> Indexs
        {
            get
            {
                int showNum = (preShowNum * 2) + 1;
                Dictionary<int, bool> indexs = [];

                int skip = -preShowNum;
                if (PageCount - CurrentIndex < preShowNum)
                {
                    //右侧将会少取到的数量
                    int rightLossCount = preShowNum - (PageCount - CurrentIndex);
                    //右侧少取的补充到左边
                    skip -= rightLossCount;
                }

                int maxLength = PageCount < showNum ? PageCount : showNum;

                while (indexs.Count < maxLength)
                {
                    int index = CurrentIndex + skip;
                    if (index > 0)
                    {
                        bool isCurrent = false;
                        if (index == CurrentIndex)
                        {
                            isCurrent = true;
                        }

                        indexs.Add(index, isCurrent);
                    }
                    skip++;
                }

                return indexs;
            }
        }

        /// <summary>
        /// 显示跳转到首页
        /// </summary>
        public bool showStart => CurrentIndex - 1 > 2;

        /// <summary>
        /// 显示跳转到上一页
        /// </summary>
        public bool showPrevious => CurrentIndex > 1;

        /// <summary>
        /// 显示跳转到下一页
        /// </summary>
        public bool showNext => CurrentIndex < PageCount;

        /// <summary>
        /// 显示跳转到尾页
        /// </summary>
        public bool showEnd => PageCount - CurrentIndex > 2;

    }
}
