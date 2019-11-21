using System;

namespace Infrastructure.common
{
    public static class PageManager
    {
        /// <summary>
        /// 获取总页数
        /// </summary>
        /// <returns>总页数</returns>
        public static int GetLastPageIndex(int count, int pageSize)
        {
            int page = (int)Math.Ceiling((decimal)count / pageSize);
            if (page == 0)
                page = 1;
            return page;
        }

        /// <summary>
        /// 获取页面显示文章的起始索引
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="count">文章数量</param>
        /// <param name="pageSize">页面大小</param>
        /// <returns></returns>
        public static int GetStartPageIndex(int index, int count, int pageSize)
        {
            int lastPage = GetLastPageIndex(count, pageSize);
            if (index >= lastPage)
            {
                index--;
            }

            if (index < 0)
            {
                index = 0;
            }

            return index;
        }
    }

}
